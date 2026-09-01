

using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Progress.Stage.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public class StageService : IStageRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsStage> _set;
      
        public StageService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsStage>();
        }
        public async Task<string> Add(clsStage entity)
        {
          await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StageInfo> GetBy(string param, enStageParamType paramType)
        {
            if (paramType == enStageParamType.ID)

                return await _set.Where(x => x.Id == clsUtil.ToGuid(param)).
                             Select(x => new StageInfo
                             {
                               StageID = x.Id.ToString(),
                               StageName = x.Name,
                               StageNote= x.Notes,
                                 ProgramName= x.Program.Name,
                               ProgramID=x.ProgramID.ToString(),
                               PrevStageID=x.PrevStageID.ToString(),
                               PrevStageName = x.PrevStage.Name
                               
                             }
                            ).FirstOrDefaultAsync();

            else
    return await _set.Where(x => x.Name == param).
                              Select(x => new StageInfo
                              {
                                  StageID = x.Id.ToString(),
                                  StageName = x.Name,
                                  StageNote = x.Notes,
                                  ProgramName = x.Program.Name,
                                  ProgramID = x.ProgramID.ToString(),
                                  PrevStageID = x.PrevStageID.ToString(),
                                  PrevStageName = x.PrevStage.Name

                              }
                            ).FirstOrDefaultAsync();
        }

        public async Task<clsStage> GetEntityBy(string param, enStageParamType paramType)
        {
            if (paramType == enStageParamType.ID)

                return await _set.Where(x => x.Id == clsUtil.ToGuid(param))
                            .FirstOrDefaultAsync();

            else
                return await _set.Where(x => x.Name == param).
                                         FirstOrDefaultAsync();
        }

        public async Task<List<StageInfo>> GetList()
        {
            return await _set.   Select(x => new StageInfo
                             {
                               StageID = x.Id.ToString(),
                               StageName = x.Name,
                               StageNote= x.Notes,
                                 ProgramName= x.Program.Name,
                               ProgramID=x.ProgramID.ToString(),
                               PrevStageID=x.PrevStageID.ToString(),
                               PrevStageName = x.PrevStage.Name
                               
                             }
                            ).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enStageParamType paramType)
        {
            if (paramType == enStageParamType.ID)

                return await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param))
                             ;

            else
                return await _set.AnyAsync(x => x.Name == param)
                                        ;
        }

        public async Task<bool> Update(clsStage entity)
        {
            _set.Update(entity);
            return true;
        }
    }
}
