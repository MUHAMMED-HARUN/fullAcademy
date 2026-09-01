using FluentValidation;
using System.Application.Services.Assignment.AssignmentScopeInfo.Command;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Validation
{
    public class UpdateAssignmentScopeInfoCommandValidation : AbstractValidator<UpdateAssignmentScopeInfoCommand>
    {
        public UpdateAssignmentScopeInfoCommandValidation()
        {
            RuleFor(x => x.AssignmentScopeInfoID)
         .NotEmpty().WithMessage("AssignmentScopeInfo ID is required");

            RuleFor(x => x.MaxScore)
                .GreaterThanOrEqualTo((short)0).WithMessage("Max score must be non-negative");

            RuleFor(x => x.MinPassScore)
                .GreaterThanOrEqualTo((short)0).WithMessage("Min pass score must be non-negative");

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0f).WithMessage("Weight must be non-negative");

            RuleFor(x => x.StageInfoID)
                .NotEmpty().WithMessage("StageInfo ID is required");

            RuleFor(x => x.AssignmentScopeID)
                .NotEmpty().WithMessage("AssignmentScope ID is required");
        }
    }
}
