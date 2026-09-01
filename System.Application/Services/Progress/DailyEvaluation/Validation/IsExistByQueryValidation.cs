using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Query;


namespace System.Application.Services.Progress.DailyEvaluation.Validator
{
    public class IsExistDailyEvaluationByQueryValidation : AbstractValidator<IsExistDailyEvaluationByQuery>
    {
        public IsExistDailyEvaluationByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
