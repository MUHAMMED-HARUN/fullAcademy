using FluentValidation;
using System.Application.Services.Assignment.AssignmentScope.Command;

namespace System.Application.Services.Assignment.AssignmentScope.Validation
{
    public class UpdateAssignmentScopeCommandValidation : AbstractValidator<UpdateAssignmentScopeCommand>
    {
        public UpdateAssignmentScopeCommandValidation()
        {
            RuleFor(x => x.AssignmentScopeID)
                .NotEmpty().WithMessage(".AssignmentScopeID  is required");
            RuleFor(x => x.Name)
          .NotEmpty().WithMessage("Assignment Scope name is required");
            RuleFor(x => x.StageID)
          .NotEmpty().WithMessage("StageID is required");
        }
    }
}
