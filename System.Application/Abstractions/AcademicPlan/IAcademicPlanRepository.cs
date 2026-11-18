using Gridify;
using System.Application.Services.AcademicPlan.AcademicPlan.DTO;
using System.Application.Services.AcademicPlan.AcademicPlan.Query;
using System.Domain.Entities.AcademicPlan;
using System.Domain.Enums.AcademicPlan;

namespace System.Application.Abstractions.AcademicPlan
{
    public interface IAcademicPlanRepository
    {
        Task<string> Add(clsAcademicPlan entity);
        Task<bool> Update(clsAcademicPlan entity);
        Task<clsAcademicPlan?> GetEntityBy(Guid id);
        Task<AcademicPlanInfo?> GetBy(Guid id);
        Task<Paging<AcademicPlanLookupInfo>> GetListFilter(GetAllAcademicPlanFilterQuery request);
        Task<AcademicPlanScopeInfo?> GetScopeAsync(enAcademicPlanScopeType scopeType, Guid scopeID);
        Task<bool> IsInstituteExist(Guid instituteID);
        Task<List<clsAcademicPlan>> GetActiveByScopeAsync(enAcademicPlanScopeType scopeType, Guid scopeID, Guid excludedAcademicPlanID);
    }
}
