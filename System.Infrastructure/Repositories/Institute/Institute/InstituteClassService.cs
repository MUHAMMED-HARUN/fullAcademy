using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
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

        public async Task<InstituteClassInfo> GetBy(string param, enInstituteClassType paramType)
        {
            if (enInstituteClassType.ClassID == paramType)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).Select(i =>
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
                return await _dbSet.Where(i => i.Class.Name == param).Select(i =>
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

        public async Task<List<InstituteClassInfo>> GetList()
        {
            return await _dbSet.Where(i=>i.DeletedAt==null).Select(i =>
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
            return await _dbSet.Where(ic=>ic.ClassID==clsUtil.ToGuid(ClassID)&& ic.InstituteID ==clsUtil.ToGuid(InstituteID)).AnyAsync();
        }

        public async Task<bool> IsExistBy(string param, enInstituteClassType paramType)
        {
            
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
      
        }

 

        public async Task<bool> Update(clsInstituteClass entity)
        {
           _dbSet.Update(entity);
            return true;
        }
    }
}
