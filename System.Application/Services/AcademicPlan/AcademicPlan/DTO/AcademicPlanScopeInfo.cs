namespace System.Application.Services.AcademicPlan.AcademicPlan.DTO
{
    public sealed class AcademicPlanScopeInfo
    {
        public Guid? ProviderInstituteID { get; set; }
        public List<Guid> StageInfoIDs { get; set; } = new();
    }
}
