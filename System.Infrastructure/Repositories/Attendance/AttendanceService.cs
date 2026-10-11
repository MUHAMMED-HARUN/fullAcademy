using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Attendance;
using System.Application.Services.Assignment.AssignmentNomination.DTO;
using System.Domain.Entities;
using System.Domain.Entities.Attendance;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace System.Infrastructure.Repositories.Attendance
{
    public class AttendanceService : IAttendanceRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAttendance> _set;

        public AttendanceService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsAttendance>();
        }

        public async Task<string> AddByRange(List<clsAttendance> entitys)
        {
          await _set.AddRangeAsync(entitys);
            return "";
        }

        public IQueryable<clsAttendance> Find(Expression<Func<clsAttendance, bool>> predicate, bool asNoTracking = true)
        {
            var query = _set.Where(predicate); 

            if(asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}
