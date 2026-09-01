using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Command;


namespace System.Application.Services.Person.Person.Validation
{
    public class UpdateDailyEvaluationCommandValidation : AbstractValidator<UpdateDailyEvaluationCommand>
    {
        public UpdateDailyEvaluationCommandValidation()
        {
            RuleFor(x => x.DailyEvaluationID).NotEmpty().WithMessage("DailyEvaluationID MustBe Not Empty");
            RuleFor(x => x.To).NotEmpty().GreaterThan(0).WithMessage("To MustBe Not Empty");
            RuleFor(x => x.LevelType).IsInEnum().NotEmpty().WithMessage("LevelType MustBe Not Empty");
        }
    }
}
