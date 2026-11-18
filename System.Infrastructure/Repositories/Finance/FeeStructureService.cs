using Microsoft.EntityFrameworkCore;
using Gridify;
using Gridify.EntityFramework;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Common.Models;
using System.Application.Services.Finance.FeeStructure.DTO;
using System.Application.Services.Finance.FeeStructure.Query;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Finance
{
    public class FeeStructureService : IFeeStructureRepository
    {
        private static readonly IGridifyMapper<FeeStructureInfo> FilterMapper =
            new GridifyMapper<FeeStructureInfo>()
                .AddMap(nameof(FeeStructureInfo.Name), x => x.Name)
                .AddMap(nameof(FeeStructureInfo.FeeType), x => x.FeeType)
                .AddMap(nameof(FeeStructureInfo.Amount), x => x.Amount)
                .AddMap(nameof(FeeStructureInfo.AcademicPlanName), x => x.AcademicPlanName)
                .AddMap(nameof(FeeStructureInfo.AcademicPlanVersion), x => x.AcademicPlanVersion)
                .AddMap(nameof(FeeStructureInfo.ProviderInstituteName), x => x.ProviderInstituteName)
                .AddMap(nameof(FeeStructureInfo.ScopeName), x => x.ScopeName)
                .AddMap(nameof(FeeStructureInfo.PlanStatus), x => x.PlanStatus);
        private readonly AppDbContext _context;
        private readonly DbSet<clsFeeStructure> _set;
        private readonly ICurrentUserService _currentUser;

        public FeeStructureService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsFeeStructure>();
            _currentUser = currentUser;
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
            var query = _set.AsNoTracking()
                .ApplyFeeStructureReadScope(_currentUser);

            return await query.Select(x => new FeeStructureInfo
            {
                FeeStructureID = x.Id.ToString(),
                Name = x.Name,
                FeeType = x.FeeType,
                Amount = x.Amount,
                AcademicPlanID = x.AcademicPlanID.ToString(),
                AcademicPlanName = x.AcademicPlan.Name,
                AcademicPlanVersion = x.AcademicPlan.Version,
                ProviderInstituteName = x.AcademicPlan.ProviderInstitute.Name,
                ScopeName = x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Program ? x.AcademicPlan.Program.Name
                    : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.InstituteClass ? x.AcademicPlan.InstituteClass.Class.Name
                    : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Stage ? x.AcademicPlan.Stage.Name
                    : x.AcademicPlan.StageInfo.Matter.Name,
                PlanStatus = (int)x.AcademicPlan.Status, HasInvoices = x.StudentInvoices.Any(),
                IsUsed = x.StudentInvoices.Any(),
                Notes = x.Notes
            }).ToListAsync();
        }

        public async Task<Paging<FeeStructureInfo>> GetListFilter(GetAllFeeStructureFilterQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);
            var projected = _set.AsNoTracking()
                .ApplyFeeStructureReadScope(_currentUser)
                .Select(x => new FeeStructureInfo
                {
                    FeeStructureID = x.Id.ToString(), Name = x.Name, FeeType = x.FeeType, Amount = x.Amount,
                    AcademicPlanID = x.AcademicPlanID.ToString(), AcademicPlanName = x.AcademicPlan.Name,
                    AcademicPlanVersion = x.AcademicPlan.Version, ProviderInstituteName = x.AcademicPlan.ProviderInstitute.Name,
                    ScopeName = x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Program ? x.AcademicPlan.Program.Name
                        : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.InstituteClass ? x.AcademicPlan.InstituteClass.Class.Name
                        : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Stage ? x.AcademicPlan.Stage.Name
                        : x.AcademicPlan.StageInfo.Matter.Name,
                    PlanStatus = (int)x.AcademicPlan.Status, HasInvoices = x.StudentInvoices.Any(), IsUsed = x.StudentInvoices.Any(), Notes = x.Notes,
                });
            return await projected.GridifyAsync(request, FilterMapper);
        }

        public Task<List<FeeStructureInfo>> GetByAcademicPlan(Guid academicPlanID) => _set.AsNoTracking()
            .ApplyFeeStructureReadScope(_currentUser)
            .Where(x => x.AcademicPlanID == academicPlanID)
            .Select(x => new FeeStructureInfo
            {
                FeeStructureID = x.Id.ToString(), Name = x.Name, FeeType = x.FeeType, Amount = x.Amount,
                AcademicPlanID = x.AcademicPlanID.ToString(), AcademicPlanName = x.AcademicPlan.Name,
                AcademicPlanVersion = x.AcademicPlan.Version, ProviderInstituteName = x.AcademicPlan.ProviderInstitute.Name,
                ScopeName = x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Program ? x.AcademicPlan.Program.Name
                    : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.InstituteClass ? x.AcademicPlan.InstituteClass.Class.Name
                    : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Stage ? x.AcademicPlan.Stage.Name
                    : x.AcademicPlan.StageInfo.Matter.Name,
                PlanStatus = (int)x.AcademicPlan.Status, HasInvoices = x.StudentInvoices.Any(), IsUsed = x.StudentInvoices.Any(), Notes = x.Notes,
            }).ToListAsync();

        public async Task<FeeStructureInfo> GetBy(string param, enFeeStructureParamType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return null;

            var query = _set.AsNoTracking()
                .ApplyFeeStructureReadScope(_currentUser);

            query = paramType switch
            {
                enFeeStructureParamType.ID when Guid.TryParse(param, out var id) => query.Where(x => x.Id == id),
                enFeeStructureParamType.Name => query.Where(x => x.Name == param),
                _ => null,
            };

            if (query == null)
                return null;

            return await query.Select(x => new FeeStructureInfo
            {
                FeeStructureID = x.Id.ToString(),
                Name = x.Name,
                FeeType = x.FeeType,
                Amount = x.Amount,
                AcademicPlanID = x.AcademicPlanID.ToString(),
                AcademicPlanName = x.AcademicPlan.Name,
                AcademicPlanVersion = x.AcademicPlan.Version,
                ProviderInstituteName = x.AcademicPlan.ProviderInstitute.Name,
                ScopeName = x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Program ? x.AcademicPlan.Program.Name
                    : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.InstituteClass ? x.AcademicPlan.InstituteClass.Class.Name
                    : x.AcademicPlan.ScopeType == System.Domain.Enums.AcademicPlan.enAcademicPlanScopeType.Stage ? x.AcademicPlan.Stage.Name
                    : x.AcademicPlan.StageInfo.Matter.Name,
                PlanStatus = (int)x.AcademicPlan.Status, HasInvoices = x.StudentInvoices.Any(),
                IsUsed = x.StudentInvoices.Any(),
                Notes = x.Notes,
            }).FirstOrDefaultAsync();
        }

        public async Task<clsFeeStructure> GetEntityBy(string param, enFeeStructureParamType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return null;

            var query = _set.ApplyFeeStructureReadScope(_currentUser);
            query = paramType switch
            {
                enFeeStructureParamType.ID when Guid.TryParse(param, out var id) => query.Where(x => x.Id == id),
                enFeeStructureParamType.Name => query.Where(x => x.Name == param),
                _ => null,
            };

            return query == null ? null : await query.FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistBy(string param, enFeeStructureParamType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return false;

            var query = _set.AsNoTracking()
                .ApplyFeeStructureReadScope(_currentUser);
            query = paramType switch
            {
                enFeeStructureParamType.ID when Guid.TryParse(param, out var id) => query.Where(x => x.Id == id),
                enFeeStructureParamType.Name => query.Where(x => x.Name == param),
                _ => null,
            };

            return query != null && await query.AnyAsync();
        }

        public Task<bool> HasInvoices(Guid feeStructureID) => _context.StudentInvoices
            .AsNoTracking().AnyAsync(invoice => invoice.FeeStructureID == feeStructureID);

        public Task<bool> IsAcademicPlanActive(Guid academicPlanID) => _context.Set<System.Domain.Entities.AcademicPlan.clsAcademicPlan>()
            .AsNoTracking().AnyAsync(plan => plan.Id == academicPlanID && plan.Status == System.Domain.Enums.AcademicPlan.enAcademicPlanStatus.Active);
    }
}
