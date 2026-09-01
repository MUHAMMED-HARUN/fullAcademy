using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Query;



namespace System.Application.Services.Progress.DailyEvaluation.Validator
{
    public class GetDailyEvaluationByQueryValidation : AbstractValidator<GetDailyEvaluationByQuery>
    {
        public GetDailyEvaluationByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
