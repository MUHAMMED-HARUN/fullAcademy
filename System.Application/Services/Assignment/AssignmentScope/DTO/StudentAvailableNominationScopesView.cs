using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScope.DTO
{
    public class StudentAvailableNominationScopesView
    {

        public Guid StudentAcademicEnrollmentID { get; set; }
        public Guid AssignmentScopeID { get; set; }
        public string AssignmentScopeName { get; set; }
    }
}
