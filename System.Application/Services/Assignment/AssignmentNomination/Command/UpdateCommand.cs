using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class UpdateAssignmentNominationCommand : ICommand, IRequest<TResult<string>>
    {
        public string AssignmentNominationID { get; set; }
        public string StudentEnrollmentID { get; set; }
        public string AssignmentScopeID { get; set; }
        public DateOnly Date { get; set; }
        public string NominatedByID { get; set; }
        public enUserType NominatedByType { get; set; }
        public enAssignmentType NominationState { get; set; }
    }
}
