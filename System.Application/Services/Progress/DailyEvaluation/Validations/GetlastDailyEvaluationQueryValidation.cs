using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Query;

namespace System.Application.Services.Progress.DailyEvaluation.Validations
{
    public sealed class GetlastDailyEvaluationQueryValidation
        : AbstractValidator<GetlastDailyEvaluationQuery>
    {
        public GetlastDailyEvaluationQueryValidation()
        {
            RuleFor(query => query.StudentAcademicEnrollmentID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID must be a valid, non-empty GUID");
            RuleFor(query => query.StageInfoID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StageInfoID must be a valid, non-empty GUID");
        }
    }
}
