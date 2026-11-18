using FluentValidation;
using System.Application.Services.Assignment.Assignment.Command;

namespace System.Application.Services.Assignment.Assignment.Validations
{
    public class DeleteAssignmentCommandValidation : AbstractValidator<DeleteAssignmentCommand>
    {
        public DeleteAssignmentCommandValidation()
        {
           
            RuleFor(x => x.AssignmentID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentID must be a valid GUID");
        }
    }
}
