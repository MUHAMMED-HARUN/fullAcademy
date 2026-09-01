using FluentValidation;
using System.Application.Services.Assignment.Assignment.Command;

namespace System.Application.Services.Assignment.Assignment.Validation
{
    public class UpdateAssignmentCommandValidation : AbstractValidator<UpdateAssignmentCommand>
    {
        public UpdateAssignmentCommandValidation()
        {
            RuleFor(x => x.AssignmentNominationID)
                .NotEmpty().WithMessage("Assignment Nomination ID is required");
            RuleFor(x => x.AssignmentID)
                .NotEmpty().WithMessage("Assignment ID is required");

            
        }
    }
}
