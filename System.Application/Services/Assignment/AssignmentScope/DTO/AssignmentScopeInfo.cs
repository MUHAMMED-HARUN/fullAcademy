using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScope.DTO
{
    public class AssignmentScopeInfo
    {
        
        public string AssignmentScopeID { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public string StageID { get; set; }
        public string StageName { get; set; }
        public string? PrevAssignmentScopeID { get; set; }
        public string? PrevAssignmentScopeName { get; set; }
    }
}
