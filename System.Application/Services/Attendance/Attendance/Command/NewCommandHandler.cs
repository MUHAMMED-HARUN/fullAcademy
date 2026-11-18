using MediatR;
using System.Application.Abstractions.Attendance;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Attendance;
using System.Domain.Enums.Attendance;

namespace System.Application.Services.Attendance.Attendance.Command
{
    public class NewAttendanceCommandHandler : IRequestHandler<NewAttendanceCommand, TResult<string>>
    {
        private readonly IAttendanceRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public NewAttendanceCommandHandler(
            IAttendanceRepository repo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(NewAttendanceCommand request, CancellationToken cancellationToken)
        {

            if (request?.Attendances == null || request.Attendances.Count == 0 || request.Date == default)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);
            if (request.Attendances.Count > 500)
                return TResult<string>.Fail("لا يمكن تسجيل أكثر من 500 عضوية في الدفعة الواحدة");

            if (!_currentUserService.HasPermission("Can_Create_Attendance"))
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var enrollmentIDs = new List<Guid>();
            Guid? batchGroupID = null;
            foreach (var attendance in request.Attendances)
            {
                if (attendance == null ||
                    !Guid.TryParse(attendance.EntityID, out var enrollmentID) || enrollmentID == Guid.Empty ||
                    attendance.AttendeeType != enAttendeeType.Student ||
                    attendance.ContextType != enContextType.Group ||
                    !Guid.TryParse(attendance.ContextID, out var groupID) || groupID == Guid.Empty)
                    return TResult<string>.Fail(ApplicationExMsg.FailAdd);

                if (batchGroupID.HasValue && batchGroupID.Value != groupID)
                    return TResult<string>.Fail("يجب أن تخص دفعة الحضور مجموعة واحدة فقط");

                batchGroupID = groupID;
                enrollmentIDs.Add(enrollmentID);
            }

            if (enrollmentIDs.Distinct().Count() != enrollmentIDs.Count)
                return TResult<string>.Fail("لا يمكن تكرار عضوية المجموعة في دفعة الحضور");

            var groupIDForBatch = batchGroupID!.Value;
            if (!await _repo.CanWriteGroupAttendance(groupIDForBatch, enrollmentIDs, request.Date))
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            var existingAttendances = await _repo.GetGroupAttendanceForUpdate(
                groupIDForBatch, enrollmentIDs, request.Date);
            var existingByContext = existingAttendances
                .GroupBy(attendance => (
                    attendance.AttendeeID,
                    attendance.Date,
                    attendance.ContextID,
                    attendance.ContextType))
                .ToDictionary(
                    group => group.Key,
                    group => group.OrderByDescending(attendance => attendance.ModifiedAt ?? attendance.CreatedAt).First());

            var hasStatusChanges = request.Attendances.Any(item =>
            {
                var enrollmentID = Guid.Parse(item.EntityID);
                var key = (enrollmentID, request.Date, (Guid?)groupIDForBatch, (enContextType?)enContextType.Group);
                return existingByContext.TryGetValue(key, out var existing) && existing.Status != item.Status;
            });
            if (hasStatusChanges && !_currentUserService.HasPermission("Can_Update_Attendance"))
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");

            var newAttendance = new List<clsAttendance>();
            foreach (var attendance in request.Attendances)
            {
                var enrollmentID = Guid.Parse(attendance.EntityID);
                var key = (enrollmentID, request.Date, (Guid?)groupIDForBatch, (enContextType?)enContextType.Group);
                if (existingByContext.TryGetValue(key, out var existing))
                {
                    if (existing.Status != attendance.Status)
                    {
                        existing.SetStatus(attendance.Status);
                        existing.SetModified(userID.ToString());
                    }
                }
                else
                {
                    var entity = new clsAttendance(
                        enrollmentID,
                        enAttendeeType.Student,
                        request.Date,
                        attendance.Status,
                        contextId: groupIDForBatch,
                        contextType: enContextType.Group);
                    entity.SetCreated(userID.ToString());
                    newAttendance.Add(entity);
                }

            }
            if (newAttendance.Count > 0)
                await _repo.AddByRange(newAttendance);
          

            return TResult<string>.OK(string.Empty);
        }
    }
}
