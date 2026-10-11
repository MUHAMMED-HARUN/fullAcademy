using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Attendance;

namespace System.Application.Services.Attendance.Attendance.Command
{
    public class NewAttendanceCommand : ICommand, IRequest<TResult<string>>
    {
      public List<AttendanceCommandDTO> Attendances { get; set; } = new List<AttendanceCommandDTO>();
    }
    public class AttendanceCommandDTO
    {
        public string EntityID { get; set; }
        public enAttendeeType AttendeeType { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
