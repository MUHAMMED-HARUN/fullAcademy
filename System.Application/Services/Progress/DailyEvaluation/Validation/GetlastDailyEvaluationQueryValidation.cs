using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Query;



namespace System.Application.Services.Progress.DailyEvaluation.Validator
{
    public class GetlastDailyEvaluationQueryValidation : AbstractValidator<GetlastDailyEvaluationQuery>
    {
        public GetlastDailyEvaluationQueryValidation()
        {
            RuleFor(x => x.StudentEnrollmentID).NotEmpty().WithMessage("StudentEnrollmentID MustBe Not Empty");
        }
    }
}
