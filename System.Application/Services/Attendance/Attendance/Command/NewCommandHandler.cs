using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Attendance;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Entities.Attendance;
using System.Domain.Enums.Attendance;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;

namespace System.Application.Services.Attendance.Attendance.Command
{
    public class NewAttendanceCommandHandler : IRequestHandler<NewAttendanceCommand, TResult<string>>
    {
        private readonly IAttendanceRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewAttendanceCommandHandler(
            IAttendanceRepository repo,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewAttendanceCommand request, CancellationToken cancellationToken)
        {

            if(request == null|| request.Attendances.Count==0 || request.Attendances==null) 
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var ToDay = DateOnly.FromDateTime(DateTime.Now).ToString();

            var AttendancIDs = request.Attendances.Select(x => clsUtil.ToGuid(x.EntityID));
            var ContextIDs = request.Attendances.Where(x => !string.IsNullOrEmpty(x.ContextID)).Select(x => clsUtil.ToGuid(x.ContextID));
           var ContextTypes = request.Attendances.Where(x => x.ContextType!=null).Select(x => x.ContextType.Value);

            var existingAttendances = _repo.Find(clsAttendance.GetByAttendees_Date_Context(AttendancIDs,
                DateOnly.FromDateTime(DateTime.Now),
                ContextIDs,
                ContextTypes
                ), false).ToDictionary(a => a.AttendeeID);
              
         List< clsAttendance >Newattendance= new List<clsAttendance>();
            foreach (var attendance in request.Attendances)
            {
                if (existingAttendances.TryGetValue(clsUtil.ToGuid(attendance.EntityID), out var toUpdate))
                {

                    if (toUpdate.Status != attendance.Status)
                    {
                        toUpdate.SetStatus(attendance.Status);
                        toUpdate.SetModified(_currentUserService.UserId.ToString());
                    }

                    if (!string.IsNullOrEmpty(attendance.ContextID))
                    {
                        toUpdate.SetContext(clsUtil.ToGuid(attendance.ContextID), attendance.ContextType);
                        toUpdate.SetModified(_currentUserService.UserId.ToString());
                    }
                }
                else
                {
                var newAttendance =    _mapper.Map<AttendanceCommandDTO, clsAttendance>(attendance);
                    newAttendance.SetCreated(_currentUserService.UserId.ToString());
                    Newattendance.Add(newAttendance);
                }

            }
            await _repo.AddByRange(Newattendance);
          

            return TResult<string>.OK(string.Empty);
        }
    }
}
