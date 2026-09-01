using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Command;


namespace System.Application.Services.Progress.DailyEvaluation.Validator
{
    public class NewDailyEvaluationCommandValidation : AbstractValidator<NewDailyEvaluationCommand>
    {

        public NewDailyEvaluationCommandValidation()
        {
            RuleFor(x => x.StudentEnrollmentID).NotEmpty().WithMessage("StudentEnrollmentID MustBe Not Empty");
            RuleFor(x => x.StageInfoID).NotEmpty().WithMessage("StageInfoID MustBe Not Empty");
            RuleFor(x => x.To).NotEmpty().GreaterThan(0).WithMessage("To MustBe Not Empty");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date MustBe Not Empty");
            RuleFor(x => x.LevelType).IsInEnum().NotEmpty().WithMessage("LevelType MustBe Not Empty");
        }
    }

}
