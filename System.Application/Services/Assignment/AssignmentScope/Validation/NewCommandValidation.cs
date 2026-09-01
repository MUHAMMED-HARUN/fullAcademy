using FluentValidation;
using System.Application.Services.Assignment.AssignmentScope.Command;

namespace System.Application.Services.Assignment.AssignmentScope.Validation
{
    public class NewAssignmentScopeCommandValidation : AbstractValidator<NewAssignmentScopeCommand>
    {
        public NewAssignmentScopeCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Assignment Scope name is required");

            RuleFor(x => x.StageID)
                .NotEmpty().WithMessage("Stage ID is required");
        }
    }
}
