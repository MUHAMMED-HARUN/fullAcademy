using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;

namespace System.Application.Services.Assignment.AssignmentNomination.Command
{
    public class NewAssignmentNominationCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentEnrollmentID { get; set; }
        public string AssignmentScopeID { get; set; }
        public string Date { get; set; }
        public string NominatedByID { get; set; }
        public enUserType NominatedByType { get; set; }
        public enAssignmentType NominationState { get; set; }
    }
}
