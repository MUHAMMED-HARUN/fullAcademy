using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Progress;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Progress.Stage.DTO;
using System.Application.Services.Progress.Stage.Query;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public class StageService : IStageRepository
    {
        private static readonly IGridifyMapper<StageLookupInfo> StageFilterMapper =
            new GridifyMapper<StageLookupInfo>()
                .AddMap(nameof(StageLookupInfo.StageName), stage => stage.StageName)
                .AddMap(nameof(StageLookupInfo.ClassName), stage => stage.ClassName)
                .AddMap(nameof(StageLookupInfo.PrevStageName), stage => stage.PrevStageName);

        private readonly AppDbContext _context;
        private readonly DbSet<clsStage> _set;
        private readonly ICurrentUserService _currentUser;
      
        public StageService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsStage>();
            _currentUser = currentUser;
        }
        public async Task<string> Add(clsStage entity)
        {
          await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StageInfo?> GetBy(string param, enStageParamType paramType)
        {
            var query = _set.AsNoTracking().ApplyStageReadScope(_currentUser, _context);
            query = paramType == enStageParamType.ID
                ? query.Where(stage => stage.Id == clsUtil.ToGuid(param))
                : query.Where(stage => stage.Name == param);

            return await ProjectStageInfo(query).FirstOrDefaultAsync();
        }

        public async Task<clsStage?> GetEntityBy(string param, enStageParamType paramType)
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
            var query = _set.AsNoTracking().ApplyStageReadScope(_currentUser, _context);
            return await ProjectStageInfo(query).ToListAsync();
        }

        public async Task<Paging<StageLookupInfo>> GetListFilter(GetAllStageFilterQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking().ApplyStageReadScope(_currentUser, _context);
            if (Guid.TryParse(request.ClassID, out var classID) && classID != Guid.Empty)
                query = query.Where(stage => stage.ClassID == classID);

            if (Guid.TryParse(request.ExcludeStageID, out var excludedStageID) && excludedStageID != Guid.Empty)
                query = query.Where(stage => stage.Id != excludedStageID);

            var projected = query.Select(stage => new StageLookupInfo
            {
                StageID = stage.Id.ToString(),
                StageName = stage.Name,
                StageNote = stage.Notes,
                ClassID = stage.ClassID.ToString(),
                ClassName = stage.Class.Name,
                PrevStageID = stage.PrevStageID == null ? null : stage.PrevStageID.ToString(),
                PrevStageName = stage.PrevStage == null ? null : stage.PrevStage.Name,
            });

            return await projected.GridifyAsync(request, StageFilterMapper);
        }

        public async Task<bool> IsExistBy(string param, enStageParamType paramType)
        {
            var query = _set.AsNoTracking().ApplyStageReadScope(_currentUser, _context);
            return paramType == enStageParamType.ID
                ? await query.AnyAsync(stage => stage.Id == clsUtil.ToGuid(param))
                : await query.AnyAsync(stage => stage.Name == param);
        }

        public async Task<bool> IsNameExistInClass(
            string name,
            Guid classID,
            Guid? excludedStageID = null)
        {
            return await _set.AnyAsync(stage =>
                stage.Name == name &&
                stage.ClassID == classID &&
                (excludedStageID == null || stage.Id != excludedStageID.Value));
        }

        public async Task<bool> IsValidPreviousStage(
            Guid classID,
            Guid? previousStageID,
            Guid? currentStageID = null)
        {
            if (previousStageID is null || previousStageID == Guid.Empty)
                return true;

            var cursor = previousStageID;
            var visited = new HashSet<Guid>();
            while (cursor is Guid currentID)
            {
                if (currentStageID == currentID || !visited.Add(currentID))
                    return false;

                var previousStage = await _set.AsNoTracking()
                    .Where(stage => stage.Id == currentID && stage.ClassID == classID)
                    .Select(stage => new { stage.Id, stage.PrevStageID })
                    .FirstOrDefaultAsync();

                if (previousStage is null)
                    return false;

                cursor = previousStage.PrevStageID;
            }

            return true;
        }

        public Task<bool> Update(clsStage entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        private static IQueryable<StageInfo> ProjectStageInfo(IQueryable<clsStage> query)
        {
            return query.Select(stage => new StageInfo
            {
                StageID = stage.Id.ToString(),
                StageName = stage.Name,
                StageNote = stage.Notes,
                ClassID = stage.ClassID.ToString(),
                ClassName = stage.Class.Name,
                PrevStageID = stage.PrevStageID == null ? null : stage.PrevStageID.ToString(),
                PrevStageName = stage.PrevStage == null ? null : stage.PrevStage.Name,
            });
        }
    }
}
