using FluentValidation;
using System.Application.Services.Assignment.Assignment.Command;

namespace System.Application.Services.Assignment.Assignment.Validation
{
    public class DeleteAssignmentCommandValidation : AbstractValidator<DeleteAssignmentCommand>
    {
        public DeleteAssignmentCommandValidation()
        {
           
            RuleFor(x => x.AssignmentID)
                .NotEmpty().WithMessage("AssignmentID is required");
        }
    }
}
