using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.FeeStructure.DTO
{
    public class FeeStructureInfo
    {
        public string FeeStructureID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public enFeeType FeeType { get; set; }
        public decimal Amount { get; set; }
        public string AcademicPlanID { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public int AcademicPlanVersion { get; set; }
        public string ProviderInstituteName { get; set; } = string.Empty;
        public string ScopeName { get; set; } = string.Empty;
        public int PlanStatus { get; set; }
        public bool HasInvoices { get; set; }
        public bool IsUsed { get; set; }
        public string? Notes { get; set; }
    }
}
