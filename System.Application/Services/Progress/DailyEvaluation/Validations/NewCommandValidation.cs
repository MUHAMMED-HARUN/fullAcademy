using FluentValidation;
using System.Application.Services.Progress.DailyEvaluation.Command;

namespace System.Application.Services.Progress.DailyEvaluation.Validations
{
    public sealed class NewDailyEvaluationCommandValidation
        : AbstractValidator<NewDailyEvaluationCommand>
    {
        public NewDailyEvaluationCommandValidation()
        {
            RuleFor(command => command.StudentAcademicEnrollmentID)
                .Must(IsGuid).WithMessage("StudentAcademicEnrollmentID must be a valid, non-empty GUID");
            RuleFor(command => command.StageInfoID)
                .Must(IsGuid).WithMessage("StageInfoID must be a valid, non-empty GUID");
            RuleFor(command => command.To).GreaterThanOrEqualTo(0);
            RuleFor(command => command.Date).NotEqual(default(DateTime));
            RuleFor(command => command.LevelType).IsInEnum();
        }

        private static bool IsGuid(string value) =>
            Guid.TryParse(value, out var id) && id != Guid.Empty;
    }
}
