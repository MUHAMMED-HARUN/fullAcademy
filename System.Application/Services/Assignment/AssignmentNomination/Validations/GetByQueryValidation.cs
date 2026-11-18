using FluentValidation;
using System.Application.Services.Assignment.AssignmentNomination.Query;

namespace System.Application.Services.Assignment.AssignmentNomination.Validations
{
    public class GetAssignmentNominationQueryValidation : AbstractValidator<GetAssignmentNominationByQuery>
    {
        public GetAssignmentNominationQueryValidation()
        {
            RuleFor(x => x.AssignmentNominationID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentNominationID must be a valid GUID");

        }
    }
}
