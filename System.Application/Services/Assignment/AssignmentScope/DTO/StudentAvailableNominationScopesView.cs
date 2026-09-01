using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScope.DTO
{
    public class StudentAvailableNominationScopesView
    {

        public Guid EnrollmentID { get; set; }
        public Guid StudentID { get; set; }
        public string? StudentName { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public Guid AssignmentScopeID { get; set; }
        public string AssignmentScopeName { get; set; }
        public DateOnly Date { get; set; }
    }
}
