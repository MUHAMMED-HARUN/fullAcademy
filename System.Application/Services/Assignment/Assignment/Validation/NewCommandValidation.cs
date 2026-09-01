using FluentValidation;
using System.Application.Services.Assignment.Assignment.Command;

namespace System.Application.Services.Assignment.Assignment.Validation
{
    public class NewAssignmentCommandValidation : AbstractValidator<NewAssignmentCommand>
    {
        public NewAssignmentCommandValidation()
        {
            RuleFor(x => x.AssignmentNominationID)
         .NotEmpty().WithMessage("Assignment Nomination ID is required");
          
        }
    }
}
