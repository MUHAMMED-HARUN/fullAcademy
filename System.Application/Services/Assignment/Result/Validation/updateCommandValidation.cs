

using FluentValidation;
using System.Application.Services.Assignment.Result.Command;

namespace System.Application.Services.Assignment.Result.Validation
{
    public class UpdateResultCommandValidation : AbstractValidator<UpdateResultCommand>
    {
        public UpdateResultCommandValidation()
        {
            RuleFor(x => x.ResultID)
                .NotEmpty().WithMessage("ResultID is required");

            RuleFor(x => x.AssignmentID)
                .NotEmpty().WithMessage("Assignment ID is required");

            RuleFor(x => x.AssignmentScopeInfoID)
                .NotEmpty().WithMessage("Assignment Scope Info ID is required");

            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0).WithMessage("Score must be non-negative");
        }
    }
}
