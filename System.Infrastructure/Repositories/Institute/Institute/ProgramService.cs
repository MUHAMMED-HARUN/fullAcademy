using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Institute.Program.DTO;
using System.Domain.Entities;
using System.Domain.Entities.Institue;
using System.Domain.Enums.Institute;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Institute.Institute
{
    public class ProgramService : IProgramRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsProgram> _dbSet;

        public ProgramService(AppDbContext appDbContext )
        {
         _context = appDbContext;
            _dbSet = _context.Set<clsProgram>();
        }

        public async Task<string> Add(clsProgram entity)
        {
          await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<ProgramInfo> GetBy(string param, enProgramType paramType)
        {
            if (paramType == enProgramType.ID)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).Select(i =>
                new ProgramInfo
                {
                    ProgramID = i.Id.ToString(),
                    ProgramName = i.Name,
                    ProgramNote= i.Notes,
                }).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(i => i.Name== param).Select(i =>
 new ProgramInfo
 {
     ProgramID = i.Id.ToString(),
     ProgramName= i.Name,
     ProgramNote= i.Notes,
 }).FirstOrDefaultAsync();
        }

        public async Task<clsProgram> GetEntityBy(string param, enProgramType paramType)
        {
            if (paramType == enProgramType.ID)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(i => i.Name == param).FirstOrDefaultAsync();
  
        }

        public async Task<List<ProgramInfo>> GetList()
        {
            return await _dbSet.Where(i=>i.DeletedAt==null).Select(i =>
                        new ProgramInfo
                        {
                            ProgramID = i.Id.ToString(),
                            ProgramName = i.Name,
                            ProgramNote = i.Notes,
                        }).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enProgramType paramType)
        {
            if (paramType == enProgramType.ID)
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
            else
                return await _dbSet.AnyAsync(i => i.Name == param);
        }

        public async Task<bool> Update(clsProgram entity)
        {
           _dbSet.Update(entity);
            return true;
        }
    }
}
