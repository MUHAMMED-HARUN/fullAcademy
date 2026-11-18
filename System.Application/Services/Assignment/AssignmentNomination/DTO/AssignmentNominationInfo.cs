using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentNomination.DTO
{
    public class AssignmentNominationInfo
    {
        
        public string AssignmentNominationID { get; set; }
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StudentName { get; set; }
        public string AcademicPlanName { get; set; } = string.Empty;
        public string AssignmentScopeID { get; set; }
        public string AssignmentScopeName { get; set; }
        public string Date { get; set; }
        public string NominationState { get; set; }
        public bool CanStartAssigment { get; set; }
    }
}
