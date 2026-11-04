using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.FeeStructure.DTO
{
    public class FeeStructureInfo
    {
        public string FeeStructureID { get; set; }
        public string Name { get; set; }
        public enFeeType FeeType { get; set; }
        public decimal Amount { get; set; }
        public string? ProgramID { get; set; }
        public string? ProgramName { get; set; }
        public string? ClassID { get; set; }
        public string? ClassName { get; set; }
        public string? StageID { get; set; }
        public string? StageName { get; set; }
        public string? StageInfoID { get; set; }
        public string? StageInfoName { get; set; }
        public string? Notes { get; set; }
    }
}
