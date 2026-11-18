using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Attendance;
using System.Application.Abstractions.Auth;
using System.Application.Services.Assignment.AssignmentNomination.DTO;
using System.Application.Services.Attendance.Attendance.DTO;
using System.Domain.Entities;
using System.Domain.Entities.Attendance;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace System.Infrastructure.Repositories.Attendance
{
    public class AttendanceService : IAttendanceRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAttendance> _set;
        private readonly ICurrentUserService _currentUser;

        public AttendanceService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsAttendance>();
            _currentUser = currentUser;
        }

        public async Task<bool> CanWriteGroupAttendance(
            Guid groupID,
            IReadOnlyCollection<Guid> enrollmentIDs,
            DateOnly date)
        {
            if (!_currentUser.IsAuthenticated || groupID == Guid.Empty || date == default ||
                enrollmentIDs == null || enrollmentIDs.Count == 0)
                return false;

            var uniqueEnrollmentIDs = enrollmentIDs.Distinct().ToArray();
            if (uniqueEnrollmentIDs.Length != enrollmentIDs.Count || uniqueEnrollmentIDs.Any(id => id == Guid.Empty))
                return false;

            if (!await _context.Groups.AsNoTracking()
                    .AnyAsync(group => group.Id == groupID && group.IsActive))
                return false;

            var memberships = await _context.StudentEnrollments.AsNoTracking()
                .ApplyAttendanceWriteScope(_currentUser, _context, date)
                .Where(enrollment => uniqueEnrollmentIDs.Contains(enrollment.Id) &&
                                     enrollment.GroupID == groupID &&
                                     enrollment.IsActive &&
                                     enrollment.Date <= date)
                .Select(enrollment => enrollment.Id)
                .ToListAsync();

            if (memberships.Count != uniqueEnrollmentIDs.Length)
                return false;

            return true;
        }

        public async Task<List<clsAttendance>> GetGroupAttendanceForUpdate(
            Guid groupID,
            IReadOnlyCollection<Guid> enrollmentIDs,
            DateOnly date)
        {
            return await _set.ApplyAttendanceReadScope(_currentUser, _context)
                .Where(attendance =>
                    attendance.Date == date &&
                    attendance.AttendeeType == System.Domain.Enums.Attendance.enAttendeeType.Student &&
                    attendance.ContextType == System.Domain.Enums.Attendance.enContextType.Group &&
                    attendance.IsActive &&
                    attendance.ContextID == groupID &&
                    enrollmentIDs.Contains(attendance.AttendeeID))
                .ToListAsync();
        }

        public async Task<List<GroupAttendanceInfo>> GetGroupAttendance(Guid groupID, DateOnly date)
        {
            if (groupID == Guid.Empty || date == default ||
                !await _context.Groups.AsNoTracking().AnyAsync(group => group.Id == groupID && group.IsActive))
                return new List<GroupAttendanceInfo>();

            var existing = await _set.AsNoTracking()
                .ApplyAttendanceReadScope(_currentUser, _context)
                .Where(attendance =>
                    attendance.Date == date &&
                    attendance.AttendeeType == System.Domain.Enums.Attendance.enAttendeeType.Student &&
                    attendance.ContextType == System.Domain.Enums.Attendance.enContextType.Group &&
                    attendance.IsActive &&
                    attendance.ContextID == groupID)
                .OrderByDescending(attendance => attendance.ModifiedAt ?? attendance.CreatedAt)
                .Select(attendance => new GroupAttendanceInfo
                {
                    GroupEnrollmentID = attendance.AttendeeID.ToString(),
                    GroupID = attendance.ContextID!.Value.ToString(),
                    Date = attendance.Date,
                    Status = attendance.Status,
                })
                .ToListAsync();
            return existing.GroupBy(record => record.GroupEnrollmentID)
                .Select(group => group.First())
                .ToList();
        }

        public async Task<string> AddByRange(List<clsAttendance> entitys)
        {
          await _set.AddRangeAsync(entitys);
            return "";
        }

        public IQueryable<clsAttendance> Find(Expression<Func<clsAttendance, bool>> predicate, bool asNoTracking = true)
        {
            IQueryable<clsAttendance> query = asNoTracking ? _set.AsNoTracking() : _set;
            return query.ApplyAttendanceReadScope(_currentUser, _context).Where(predicate);
        }
    }
}
