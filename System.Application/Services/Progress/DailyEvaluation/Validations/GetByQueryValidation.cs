using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Query;

namespace System.Application.Services.Progress.DailyEvaluation.Validations
{
    public sealed class GetDailyEvaluationByQueryValidation
        : AbstractValidator<GetDailyEvaluationByQuery>
    {
        public GetDailyEvaluationByQueryValidation()
        {
            RuleFor(query => query.Param)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("Param must be a valid, non-empty GUID");
            RuleFor(query => query.ParamType).IsInEnum();
        }
    }
}
