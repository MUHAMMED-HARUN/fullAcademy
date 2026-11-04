using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.InstituteClass.DTO;
using System.Application.Services.Institute.Program.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Institute;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Institute.Institute
{
    public class InstituteClassService : IInstituteClassRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsInstituteClass> _dbSet;

        public InstituteClassService(AppDbContext appDbContext )
        {
         _context = appDbContext;
            _dbSet = _context.Set<clsInstituteClass>();
        }

        public async Task<string> Add(clsInstituteClass entity)
        {
          await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<InstituteClassInfo> GetBy(
            string param,
            enInstituteClassType paramType,
            InstituteClassReadScope readScope)
        {
            if (enInstituteClassType.ClassID == paramType)
                return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                    .Where(i => i.Id == clsUtil.ToGuid(param)).Select(i =>
                new InstituteClassInfo
                {
                    InstituteClassID = i.Id.ToString(),
                    ClassID = i.ClassID.ToString(),
                    InstituteID = i.InstituteID.ToString(),
                    ClassName = i.Class.Name,
                    InstituteName = i.Institute.Name,
                    ClassLevel = i.Class.Level
                }).FirstOrDefaultAsync();
            else if (enInstituteClassType.ClassName == paramType)
                return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                    .Where(i => i.Class.Name == param).Select(i =>
         new InstituteClassInfo
         {
             InstituteClassID = i.Id.ToString(),
             ClassID = i.ClassID.ToString(),
             InstituteID = i.InstituteID.ToString(),
             ClassName = i.Class.Name,
             InstituteName = i.Institute.Name,
             ClassLevel = i.Class.Level
         }).FirstOrDefaultAsync();
            else
                return default; 

        }

        public async Task<clsInstituteClass> GetEntityBy(string param, enInstituteClassType paramType)
        {
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
        }

        public async Task<List<InstituteClassInfo>> GetList(InstituteClassReadScope readScope)
        {
            return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                .Where(i => i.DeletedAt == null).Select(i =>
                                 new InstituteClassInfo
                                 {
                                     InstituteClassID = i.Id.ToString(),
                                     ClassID = i.ClassID.ToString(),
                                     InstituteID = i.InstituteID.ToString(),
                                     ClassName = i.Class.Name,
                                     InstituteName = i.Institute.Name,
                                     ClassLevel = i.Class.Level
                                 }).ToListAsync();
        }

        public async Task<bool> IsClassInInstituteBy(string ClassID, string InstituteID)
        {
            if (!Guid.TryParse(ClassID, out var classID) || classID == Guid.Empty ||
                !Guid.TryParse(InstituteID, out var instituteID) || instituteID == Guid.Empty)
            {
                return false;
            }

            return (await GetAlreadyLinkedClassIDs(instituteID, new[] { classID })).Count > 0;
        }

        public async Task<List<Guid>> GetAlreadyLinkedClassIDs(
            Guid instituteID,
            IReadOnlyCollection<Guid> classIDs)
        {
            if (classIDs.Count == 0)
                return new List<Guid>();

            // The composite unique index includes soft-deleted rows; check those too before inserting.
            return await _dbSet.IgnoreQueryFilters()
                .Where(link => link.InstituteID == instituteID && classIDs.Contains(link.ClassID))
                .Select(link => link.ClassID)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enInstituteClassType paramType)
        {
            
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
      
        }

        public async Task<bool> IsExistBy(
            string param,
            enInstituteClassType paramType,
            InstituteClassReadScope readScope)
        {
            var query = ApplyReadScope(_dbSet.AsNoTracking(), readScope);
            return await query.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
        }

        private IQueryable<clsInstituteClass> ApplyReadScope(
            IQueryable<clsInstituteClass> query,
            InstituteClassReadScope readScope)
        {
            if (readScope.IsAdmin)
                return query;

            if (readScope.TeacherID is Guid teacherID && teacherID != Guid.Empty)
            {
                return query.Where(link => _context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID &&
                    assignment.Group.InstituteClassID == link.Id));
            }

            if (readScope.StudentID is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(link => _context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentID &&
                    enrollment.Group.InstituteClassID == link.Id));
            }

            return query.Where(_ => false);
        }

 

        public async Task<bool> Update(clsInstituteClass entity)
        {
           _dbSet.Update(entity);
            return true;
        }
    }
}
