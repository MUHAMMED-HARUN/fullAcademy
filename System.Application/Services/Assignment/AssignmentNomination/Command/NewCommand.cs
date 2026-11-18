using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Assignment.AssignmentNomination.Command
{
    public class NewAssignmentNominationCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string AssignmentScopeID { get; set; }
        public string Date { get; set; }
    }
}
