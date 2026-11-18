using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.AcademicPlan;
using System.Application.Abstractions.Auth;
using System.Application.Services.AcademicPlan.AcademicPlan.DTO;
using System.Application.Services.AcademicPlan.AcademicPlan.Query;
using System.Domain.Entities.AcademicPlan;
using System.Domain.Enums.AcademicPlan;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.AcademicPlan
{
    public class AcademicPlanService : IAcademicPlanRepository
    {
        private static readonly IGridifyMapper<AcademicPlanLookupInfo> AcademicPlanFilterMapper =
            new GridifyMapper<AcademicPlanLookupInfo>()
                .AddMap(nameof(AcademicPlanLookupInfo.Name), plan => plan.Name)
                .AddMap(nameof(AcademicPlanLookupInfo.ProviderInstituteID), plan => plan.ProviderInstituteID)
                .AddMap(nameof(AcademicPlanLookupInfo.ProviderInstituteName), plan => plan.ProviderInstituteName)
                .AddMap(nameof(AcademicPlanLookupInfo.ScopeName), plan => plan.ScopeName)
                .AddMap(nameof(AcademicPlanLookupInfo.Status), plan => plan.Status);

        private readonly AppDbContext _context;
        private readonly DbSet<clsAcademicPlan> _set;
        private readonly ICurrentUserService _currentUser;

        public AcademicPlanService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsAcademicPlan>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsAcademicPlan entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public Task<bool> Update(clsAcademicPlan entity)
        {
            return Task.FromResult(true);
        }

        public async Task<clsAcademicPlan?> GetEntityBy(Guid id)
        {
            return await _set.Include(plan => plan.Items)
                .ApplyAcademicPlanReadScope(_currentUser)
                .FirstOrDefaultAsync(plan => plan.Id == id);
        }

        public async Task<AcademicPlanInfo?> GetBy(Guid id)
        {
            return await _set.AsNoTracking()
                .ApplyAcademicPlanReadScope(_currentUser)
                .Where(plan => plan.Id == id)
                .Select(plan => new AcademicPlanInfo
                {
                    AcademicPlanID = plan.Id.ToString(),
                    Name = plan.Name,
                    ProviderInstituteID = plan.ProviderInstituteID.ToString(),
                    ProviderInstituteName = plan.ProviderInstitute.Name,
                    ScopeType = plan.ScopeType,
                    ProgramID = plan.ProgramID == null ? null : plan.ProgramID.ToString(),
                    ProgramName = plan.Program != null ? plan.Program.Name : null,
                    InstituteClassID = plan.InstituteClassID == null ? null : plan.InstituteClassID.ToString(),
                    InstituteClassName = plan.InstituteClass != null ? plan.InstituteClass.Class.Name : null,
                    StageID = plan.StageID == null ? null : plan.StageID.ToString(),
                    StageName = plan.Stage != null ? plan.Stage.Name : null,
                    StageInfoID = plan.StageInfoID == null ? null : plan.StageInfoID.ToString(),
                    StageInfoName = plan.StageInfo != null ? plan.StageInfo.Matter.Name : null,
                    Version = plan.Version,
                    Status = plan.Status,
                    EffectiveFrom = plan.EffectiveFrom,
                    EffectiveTo = plan.EffectiveTo,
                    Notes = plan.Notes,
                    Items = plan.Items.Select(item => new AcademicPlanItemInfo
                    {
                        StageInfoID = item.StageInfoID.ToString(),
                        StageInfoName = item.StageInfo.Matter.Name,
                        StageID = item.StageInfo.StageID.ToString(),
                        StageName = item.StageInfo.Stage.Name,
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Paging<AcademicPlanLookupInfo>> GetListFilter(GetAllAcademicPlanFilterQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var projected = _set.AsNoTracking()
                .ApplyAcademicPlanReadScope(_currentUser)
                .Select(plan => new AcademicPlanLookupInfo
                {
                    AcademicPlanID = plan.Id.ToString(),
                    Name = plan.Name,
                    ProviderInstituteID = plan.ProviderInstituteID.ToString(),
                    ProviderInstituteName = plan.ProviderInstitute.Name,
                    ScopeType = plan.ScopeType,
                    ScopeID = plan.ScopeType == enAcademicPlanScopeType.Program ? plan.ProgramID.ToString()
                        : plan.ScopeType == enAcademicPlanScopeType.InstituteClass ? plan.InstituteClassID.ToString()
                        : plan.ScopeType == enAcademicPlanScopeType.Stage ? plan.StageID.ToString()
                        : plan.StageInfoID.ToString(),
                    ScopeName = plan.ScopeType == enAcademicPlanScopeType.Program ? plan.Program.Name
                        : plan.ScopeType == enAcademicPlanScopeType.InstituteClass ? plan.InstituteClass.Class.Name
                        : plan.ScopeType == enAcademicPlanScopeType.Stage ? plan.Stage.Name
                        : plan.StageInfo.Matter.Name,
                    Version = plan.Version,
                    Status = plan.Status,
                    EffectiveFrom = plan.EffectiveFrom,
                    EffectiveTo = plan.EffectiveTo,
                });

            return await projected.GridifyAsync(request, AcademicPlanFilterMapper);
        }

        public async Task<AcademicPlanScopeInfo?> GetScopeAsync(enAcademicPlanScopeType scopeType, Guid scopeID)
        {
            switch (scopeType)
            {
                case enAcademicPlanScopeType.Program:
                    if (!await _context.Programs.AnyAsync(program => program.Id == scopeID))
                        return null;

                    var programClassIDs = _context.Classes
                        .Where(schoolClass => schoolClass.ProgramID == scopeID)
                        .Select(schoolClass => schoolClass.Id);
                    var programStageIDs = _context.Stages
                        .Where(stage => programClassIDs.Contains(stage.ClassID))
                        .Select(stage => stage.Id);

                    return new AcademicPlanScopeInfo
                    {
                        StageInfoIDs = await _context.StageInfos
                            .Where(stageInfo => programStageIDs.Contains(stageInfo.StageID))
                            .Select(stageInfo => stageInfo.Id)
                            .ToListAsync(),
                    };

                case enAcademicPlanScopeType.InstituteClass:
                    var instituteClass = await _context.InstituteClasses
                        .Where(item => item.Id == scopeID)
                        .Select(item => new { item.InstituteID, item.ClassID })
                        .FirstOrDefaultAsync();
                    if (instituteClass == null)
                        return null;

                    var classStageIDs = _context.Stages
                        .Where(stage => stage.ClassID == instituteClass.ClassID)
                        .Select(stage => stage.Id);

                    return new AcademicPlanScopeInfo
                    {
                        ProviderInstituteID = instituteClass.InstituteID,
                        StageInfoIDs = await _context.StageInfos
                            .Where(stageInfo => classStageIDs.Contains(stageInfo.StageID))
                            .Select(stageInfo => stageInfo.Id)
                            .ToListAsync(),
                    };

                case enAcademicPlanScopeType.Stage:
                    if (!await _context.Stages.AnyAsync(stage => stage.Id == scopeID))
                        return null;

                    return new AcademicPlanScopeInfo
                    {
                        StageInfoIDs = await _context.StageInfos
                            .Where(stageInfo => stageInfo.StageID == scopeID)
                            .Select(stageInfo => stageInfo.Id)
                            .ToListAsync(),
                    };

                case enAcademicPlanScopeType.StageInfo:
                    if (!await _context.StageInfos.AnyAsync(stageInfo => stageInfo.Id == scopeID))
                        return null;

                    return new AcademicPlanScopeInfo { StageInfoIDs = new List<Guid> { scopeID } };

                default:
                    return null;
            }
        }

        public async Task<bool> IsInstituteExist(Guid instituteID)
        {
            return await _context.Institutes.AnyAsync(institute => institute.Id == instituteID);
        }

        public async Task<List<clsAcademicPlan>> GetActiveByScopeAsync(
            enAcademicPlanScopeType scopeType,
            Guid scopeID,
            Guid excludedAcademicPlanID)
        {
            var query = _set.Where(plan =>
                plan.Status == enAcademicPlanStatus.Active &&
                plan.Id != excludedAcademicPlanID);

            query = scopeType switch
            {
                enAcademicPlanScopeType.Program => query.Where(plan => plan.ProgramID == scopeID),
                enAcademicPlanScopeType.InstituteClass => query.Where(plan => plan.InstituteClassID == scopeID),
                enAcademicPlanScopeType.Stage => query.Where(plan => plan.StageID == scopeID),
                enAcademicPlanScopeType.StageInfo => query.Where(plan => plan.StageInfoID == scopeID),
                _ => query.Where(_ => false),
            };

            return await query.ToListAsync();
        }
    }
}
