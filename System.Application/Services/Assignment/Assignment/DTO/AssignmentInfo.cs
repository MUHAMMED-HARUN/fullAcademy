using System.Domain.Entities;

namespace System.Application.Services.Assignment.Assignment.DTO
{
    public class AssignmentInfo
    {
        public string AssignmentID { get; set; }
        public string AssignmentScopeID { get; set; }
        public string AssignmentNominationID { get; set; }
        public string ScheduledDate { get; set; }
        public string AssignmentState { get; set; }
        public string StudentName {  get; set; }
        public string AssigmentScopeName{  get; set; }
        public bool? IsPassed { get; set; }
    }
}
