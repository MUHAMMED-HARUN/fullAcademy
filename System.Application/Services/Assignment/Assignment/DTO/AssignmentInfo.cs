using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.DTO
{
    public class AssignmentInfo
    {
        public string AssignmentID { get; set; } = string.Empty;
        public string AssignmentScopeID { get; set; } = string.Empty;
        public string AssignmentNominationID { get; set; } = string.Empty;
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public string ScheduledDate { get; set; } = string.Empty;
        public string AssignmentState { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AssigmentScopeName { get; set; } = string.Empty;
        public string CommitteeID { get; set; } = string.Empty;
        public enCommitteeType CommitteeType { get; set; }
        public bool? IsPassed { get; set; }
        public bool CanAddResult { get; set; }
        public double? TotalScore { get; set; }
    }
}
