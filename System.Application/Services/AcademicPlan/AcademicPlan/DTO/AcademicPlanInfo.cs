using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.DTO
{
    public sealed class AcademicPlanInfo
    {
        public string AcademicPlanID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProviderInstituteID { get; set; } = string.Empty;
        public string ProviderInstituteName { get; set; } = string.Empty;
        public enAcademicPlanScopeType ScopeType { get; set; }
        public string? ProgramID { get; set; }
        public string? ProgramName { get; set; }
        public string? InstituteClassID { get; set; }
        public string? InstituteClassName { get; set; }
        public string? StageID { get; set; }
        public string? StageName { get; set; }
        public string? StageInfoID { get; set; }
        public string? StageInfoName { get; set; }
        public int Version { get; set; }
        public enAcademicPlanStatus Status { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
        public string? Notes { get; set; }
        public List<AcademicPlanItemInfo> Items { get; set; } = new();
    }

    public sealed class AcademicPlanItemInfo
    {
        public string StageInfoID { get; set; } = string.Empty;
        public string StageInfoName { get; set; } = string.Empty;
        public string StageID { get; set; } = string.Empty;
        public string StageName { get; set; } = string.Empty;
    }
}
