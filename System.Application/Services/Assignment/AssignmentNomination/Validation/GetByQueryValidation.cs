using FluentValidation;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.AssignmentNomination.Query;

namespace System.Application.Services.Assignment.AssignmentNomination.Validation
{
    public class GetAssignmentNominationQueryValidation : AbstractValidator<GetAssignmentNominationByQuery>
    {
        public GetAssignmentNominationQueryValidation()
        {
            RuleFor(x => x.AssignmentNominationID)
    .NotEmpty().WithMessage("AssignmentNomination ID is required");

        }
    }
}
