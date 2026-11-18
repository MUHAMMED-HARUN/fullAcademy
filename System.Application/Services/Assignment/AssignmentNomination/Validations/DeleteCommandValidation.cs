using FluentValidation;
using System.Application.Services.Assignment.AssignmentNomination.Command;

namespace System.Application.Services.Assignment.AssignmentNomination.Validations
{
    public sealed class DeleteAssignmentNominationCommandValidation : AbstractValidator<DeleteAssignmentNominationCommand>
    {
        public DeleteAssignmentNominationCommandValidation()
        {
            RuleFor(command => command.AssignmentNominationID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentNominationID must be a valid GUID");
        }
    }
}
