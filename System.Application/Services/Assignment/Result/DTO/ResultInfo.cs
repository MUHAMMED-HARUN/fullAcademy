using System.Domain.Entities;

namespace System.Application.Services.Assignment.Result.DTO
{
    public class ResultInfo
    {
        public string ResultID { get; set; } 
        public string AssignmentID { get; set; }
        public string StudentName { get; set; }
        public string AssignmentScopeName { get; set; }
        public string AssignmentScopeInfoID { get; set; }
        public string StageInfoMatter { get; set; }
        public double Score { get; set; }
        public string? Notes { get; set; }
    }
}
