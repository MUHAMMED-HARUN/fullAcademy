using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Command;


namespace System.Application.Services.Progress.DailyEvaluation.Validator
{
    public class DeleteDailyEvaluationCommandValidation : AbstractValidator<DeleteDailyEvaluationCommand>
    {
        public DeleteDailyEvaluationCommandValidation()
        {
            RuleFor(x => x.DailyEvaluationID).NotEmpty().WithMessage("DailyEvaluation id MustBe Not Empty");
        }
    }
}
