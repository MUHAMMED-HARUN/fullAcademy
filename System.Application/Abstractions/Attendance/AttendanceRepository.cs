using System.Domain.Entities.Attendance;

namespace System.Application.Abstractions.Attendance
{
    public interface IAttendanceRepository : IGeneric<clsAttendance>
    {
        Task<string> AddByRange(List< clsAttendance> entitys);
    }
}
