using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Attendance;

namespace System.Application.Services.Attendance.Attendance.Command
{
    public class NewAttendanceCommand : ICommand, IRequest<TResult<string>>
    {
        public List<AttendanceCommandDTO> Attendances { get; set; } = new();
        public DateOnly Date { get; set; }
    }
    public class AttendanceCommandDTO
    {
        // For Student/Group attendance this is the Group membership (StudentEnrollment) ID, never StudentID.
        public string EntityID { get; set; } = string.Empty;
        public enAttendeeType AttendeeType { get; set; } = enAttendeeType.Student;
        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
        public string? ContextID { get; set; }
        public enContextType? ContextType { get; set; }
    }
}
