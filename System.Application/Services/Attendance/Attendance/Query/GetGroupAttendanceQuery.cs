using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Attendance.Attendance.DTO;

namespace System.Application.Services.Attendance.Attendance.Query
{
    public sealed class GetGroupAttendanceQuery : IRequest<TResult<List<GroupAttendanceInfo>>>, IQuery
    {
        public string GroupID { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}
