using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Query;

namespace System.Application.Services.Progress.DailyEvaluation.Validations
{
    public sealed class GetAllDailyEvaluationQueryValidation
        : AbstractValidator<GetAllDailyEvaluationQuery>
    {
        public GetAllDailyEvaluationQueryValidation()
        {
            RuleFor(query => query.StudentAcademicEnrollmentID)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                               (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("StudentAcademicEnrollmentID must be a valid GUID");
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
        }
    }
}
