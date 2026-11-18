using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
namespace System.Application.Services.Assignment.AssignmentNomination.Command
{
    public class UpdateAssignmentNominationCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentNominationID { get; set; }
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string AssignmentScopeID { get; set; }
        public DateOnly Date { get; set; }
    }
}
