using MediatR;
using System.Application.Abstractions.Attendance;
using System.Application.Common;
using System.Application.Services.Attendance.Attendance.DTO;
using System.Application.Services.Attendance.Attendance.Query;

namespace System.Application.Services.Attendance.Attendance.Query
{
    public sealed class GetGroupAttendanceQueryHandler
        : IRequestHandler<GetGroupAttendanceQuery, TResult<List<GroupAttendanceInfo>>>
    {
        private readonly IAttendanceRepository _repository;

        public GetGroupAttendanceQueryHandler(IAttendanceRepository repository) => _repository = repository;

        public async Task<TResult<List<GroupAttendanceInfo>>> Handle(
            GetGroupAttendanceQuery request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.GroupID, out var groupID) || groupID == Guid.Empty || request.Date == default)
                return TResult<List<GroupAttendanceInfo>>.Fail("GroupID and Date are required");

            return TResult<List<GroupAttendanceInfo>>.OK(
                await _repository.GetGroupAttendance(groupID, request.Date));
        }
    }
}
