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

        public async Task<ClassInfo> GetBy(string param, enClassType paramType)
        {
            if (paramType == enClassType.ID)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).Select(i =>
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
                return await _dbSet.Where(i => i.Name== param).Select(i =>
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

        public async Task<List<ClassInfo>> GetList()
        {
            return await _dbSet.Where(i=>i.DeletedAt==null).Select(i =>
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

        public async Task<bool> Update(clsClass entity)
        {
           _dbSet.Update(entity);
            return true;
        }
    }
}
