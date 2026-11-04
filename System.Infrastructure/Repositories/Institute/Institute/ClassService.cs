using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Class.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Institute;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Institute.Institute
{
    public class ClassService : IClassRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsClass> _dbSet;

        public ClassService(AppDbContext appDbContext )
        {
         _context = appDbContext;
            _dbSet = _context.Set<clsClass>();
        }

        public async Task<string> Add(clsClass entity)
        {
          await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<ClassInfo> GetBy(
            string param,
            enClassType paramType,
            ClassReadScope readScope)
        {
            if (paramType == enClassType.ID)
                return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                    .Where(i => i.Id == clsUtil.ToGuid(param)).Select(i =>
                new ClassInfo
                {
                    ClassID = i.Id.ToString(),
                    ClassName = i.Name,
                    ClassNotes = i.Note,
                    ClassLevel = i.Level,
                    ProgramID = i.ProgramID.ToString(),
                    ProgramName= i.Program.Name,
                }).FirstOrDefaultAsync();
            else
                return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                    .Where(i => i.Name== param).Select(i =>
    new ClassInfo
    {
        ClassID = i.Id.ToString(),
        ClassName = i.Name,
        ClassNotes = i.Note,
        ClassLevel = i.Level,
        ProgramID = i.ProgramID.ToString(),
        ProgramName = i.Program.Name,
    }).FirstOrDefaultAsync();
        }

        public async Task<clsClass> GetEntityBy(string param, enClassType paramType)
        {
            if (paramType == enClassType.ID)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(i => i.Name == param).FirstOrDefaultAsync();
  
        }

        public async Task<List<ClassInfo>> GetList(ClassReadScope readScope)
        {
            return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                .Where(i => i.DeletedAt == null).Select(i =>
                          new ClassInfo
                          {
                              ClassID = i.Id.ToString(),
                              ClassName = i.Name,
                              ClassNotes = i.Note,
                              ClassLevel = i.Level,
                              ProgramID = i.ProgramID.ToString(),
                              ProgramName = i.Program.Name,
                          }).ToListAsync();
        }

        public async Task<bool> IsAvalibleLevel(int Level, string programID)
        {
            return await _dbSet.AnyAsync(c => c.Level == Level && c.ProgramID == clsUtil.ToGuid(programID));
        }

        public async Task<bool> IsAvalibleName(string name, int? Level = null, string programID = null)
        {
            return await _dbSet.AnyAsync(c =>
        (Level == null || c.Level == Level) &&
        (programID == null || c.ProgramID == clsUtil.ToGuid(programID) &&
        c.Name == name)
    );
        }

        public async Task<bool> IsExistBy(string param, enClassType paramType)
        {
            if (paramType == enClassType.ID)
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
            else
                return await _dbSet.AnyAsync(i => i.Name == param);
        }

        public async Task<List<Guid>> GetExistingClassIDs(
            IReadOnlyCollection<Guid> classIDs,
            ClassReadScope readScope)
        {
            if (classIDs.Count == 0)
                return new List<Guid>();

            return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                .Where(cls => classIDs.Contains(cls.Id))
                .Select(cls => cls.Id)
                .ToListAsync();
        }

        public async Task<bool> IsExistBy(
            string param,
            enClassType paramType,
            ClassReadScope readScope)
        {
            var query = ApplyReadScope(_dbSet.AsNoTracking(), readScope);
            return paramType == enClassType.ID
                ? await query.AnyAsync(cls => cls.Id == clsUtil.ToGuid(param))
                : await query.AnyAsync(cls => cls.Name == param);
        }

        private IQueryable<clsClass> ApplyReadScope(
            IQueryable<clsClass> query,
            ClassReadScope readScope)
        {
            if (readScope.IsAdmin)
                return query;

            if (readScope.TeacherID is Guid teacherID && teacherID != Guid.Empty)
            {
                return query.Where(cls => _context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID &&
                    assignment.Group.InstituteClass.ClassID == cls.Id));
            }

            if (readScope.StudentID is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(cls => _context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentID &&
                    enrollment.Group.InstituteClass.ClassID == cls.Id));
            }

            return query.Where(_ => false);
        }

        public async Task<bool> Update(clsClass entity)
        {
           _dbSet.Update(entity);
            return true;
        }
    }
}
