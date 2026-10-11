namespace System.Domain.Enums.Attendance
{
    public enum enAttendeeType
    {
        Student = 1,
        Teacher = 2,
        Employee = 3
    }

    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Late = 3,
        Excused = 4
    }

    public enum LeaveType
    {
        Sick = 1,
        Personal = 2,
        Emergency = 3,
        Unpaid = 4
    }

    public enum LeaveStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4
    }
}