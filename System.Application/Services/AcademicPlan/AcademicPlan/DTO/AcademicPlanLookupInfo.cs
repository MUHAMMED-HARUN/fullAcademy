using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.DTO
{
    public sealed class AcademicPlanLookupInfo
    {
        public string AcademicPlanID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProviderInstituteID { get; set; } = string.Empty;
        public string ProviderInstituteName { get; set; } = string.Empty;
        public enAcademicPlanScopeType ScopeType { get; set; }
        public string ScopeID { get; set; } = string.Empty;
        public string ScopeName { get; set; } = string.Empty;
        public int Version { get; set; }
        public enAcademicPlanStatus Status { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
    }
}
