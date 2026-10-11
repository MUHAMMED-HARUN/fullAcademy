using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.DTO
{
    public class AssignmentScopeInfoInfo
    {

        public string AssignmentScopeInfoID { get; set; }
        public short MaxScore { get; set; }
        public short MinPassScore { get; set; }
        public float Weight { get; set; }
        public string StageInfoID { get; set; }
        public string StageInfoMatterNameName { get; set; }
        public string AssignmentScopeID { get; set; }
        public string AssignmentScopeName { get; set; }
    }
}
