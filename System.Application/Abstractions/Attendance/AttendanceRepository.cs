using System.Domain.Entities.Attendance;
using System.Application.Services.Attendance.Attendance.DTO;

namespace System.Application.Abstractions.Attendance
{
    public interface IAttendanceRepository : IGeneric<clsAttendance>
    {
        Task<string> AddByRange(List< clsAttendance> entitys);
        Task<bool> CanWriteGroupAttendance(
            Guid groupID,
            IReadOnlyCollection<Guid> enrollmentIDs,
            DateOnly date);
        Task<List<clsAttendance>> GetGroupAttendanceForUpdate(
            Guid groupID,
            IReadOnlyCollection<Guid> enrollmentIDs,
            DateOnly date);
        Task<List<GroupAttendanceInfo>> GetGroupAttendance(Guid groupID, DateOnly date);
    }
}
