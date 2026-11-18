using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Command;

namespace System.Application.Services.Progress.DailyEvaluation.Validations
{
    public sealed class DeleteDailyEvaluationCommandValidation
        : AbstractValidator<DeleteDailyEvaluationCommand>
    {
        public DeleteDailyEvaluationCommandValidation()
        {
            RuleFor(command => command.DailyEvaluationID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("DailyEvaluationID must be a valid, non-empty GUID");
        }
    }
}
