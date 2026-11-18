using System.Domain.Enums.Attendance;

namespace System.Application.Services.Attendance.Attendance.DTO
{
    public sealed class GroupAttendanceInfo
    {
        public string GroupEnrollmentID { get; set; } = string.Empty;
        public string GroupID { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
