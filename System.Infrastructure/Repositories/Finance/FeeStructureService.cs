using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Finance
{
    public class FeeStructureService : IFeeStructureRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsFeeStructure> _set;

        public FeeStructureService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsFeeStructure>();
        }

        public async Task<string> Add(clsFeeStructure entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsFeeStructure entity)
        {
            _set.Update(entity);
            return true;
        }

        public async Task<List<FeeStructureInfo>> GetList()
        {
            return await _set.Select(x => new FeeStructureInfo
            {
                FeeStructureID = x.Id.ToString(),
                Name = x.Name,
                FeeType = x.FeeType,
                Amount = x.Amount,
                ProgramID = x.ProgramID.ToString(),
                ProgramName = x.Program != null ? x.Program.Name : null,
                ClassID = x.ClassID.ToString(),
                ClassName = x.Class != null ? x.Class.Name : null,
                StageID = x.StageID.ToString(),
                StageName = x.Stage != null ? x.Stage.Name : null,
                StageInfoID = x.StageInfoID.ToString(),
                StageInfoName = x.StageInfo != null && x.StageInfo.Matter != null ? x.StageInfo.Matter.Name : null,
                Notes = x.Notes
            }).ToListAsync();
        }

        public async Task<FeeStructureInfo> GetBy(string param, enFeeStructureParamType paramType)
        {
            return paramType switch
            {
                enFeeStructureParamType.ID => await _set
                    .Where(x => x.Id == clsUtil.ToGuid(param))
                    .Select(x => new FeeStructureInfo
                    {
                        FeeStructureID = x.Id.ToString(),
                        Name = x.Name,
                        FeeType = x.FeeType,
                        Amount = x.Amount,
                        ProgramID = x.ProgramID.ToString(),
                        ProgramName = x.Program != null ? x.Program.Name : null,
                        ClassID = x.ClassID.ToString(),
                        ClassName = x.Class != null ? x.Class.Name : null,
                        StageID = x.StageID.ToString(),
                        StageName = x.Stage != null ? x.Stage.Name : null,
                        StageInfoID = x.StageInfoID.ToString(),
                        Notes = x.Notes
                    }).FirstOrDefaultAsync(),

                enFeeStructureParamType.Name => await _set
                    .Where(x => x.Name == param)
                    .Select(x => new FeeStructureInfo
                    {
                        FeeStructureID = x.Id.ToString(),
                        Name = x.Name,
                        FeeType = x.FeeType,
                        Amount = x.Amount,
                        ProgramID = x.ProgramID.ToString(),
                        ProgramName = x.Program != null ? x.Program.Name : null,
                        ClassID = x.ClassID.ToString(),
                        ClassName = x.Class != null ? x.Class.Name : null,
                        StageID = x.StageID.ToString(),
                        StageName = x.Stage != null ? x.Stage.Name : null,
                        StageInfoID = x.StageInfoID.ToString(),
                        Notes = x.Notes
                    }).FirstOrDefaultAsync(),

                _ => null
            };
        }

        public async Task<clsFeeStructure> GetEntityBy(string param, enFeeStructureParamType paramType)
        {
            return paramType switch
            {
                enFeeStructureParamType.ID => await _set.FirstOrDefaultAsync(x => x.Id == clsUtil.ToGuid(param)),
                enFeeStructureParamType.Name => await _set.FirstOrDefaultAsync(x => x.Name == param),
                _ => null
            };
        }

        public async Task<bool> IsExistBy(string param, enFeeStructureParamType paramType)
        {
            return paramType switch
            {
                enFeeStructureParamType.ID => await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param)),
                enFeeStructureParamType.Name => await _set.AnyAsync(x => x.Name == param),
                _ => false
            };
        }
    }
}
