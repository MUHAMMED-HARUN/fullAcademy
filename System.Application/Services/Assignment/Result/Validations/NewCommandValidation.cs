using FluentValidation;
using System.Application.Services.Assignment.Result.Command;

namespace System.Application.Services.Assignment.Result.Validations
{
    public class NewResultCommandValidation : AbstractValidator<NewResultCommand>
    {
        public NewResultCommandValidation()
        {
            RuleFor(x => x.AssignmentID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentID must be a valid GUID");

            RuleFor(x => x.AssignmentScopeInfoID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentScopeInfoID must be a valid GUID");

            RuleFor(x => x.Score)
                .Must(double.IsFinite).WithMessage("Score must be finite")
                .GreaterThanOrEqualTo(0).WithMessage("Score must be non-negative");
        }
    }
}
