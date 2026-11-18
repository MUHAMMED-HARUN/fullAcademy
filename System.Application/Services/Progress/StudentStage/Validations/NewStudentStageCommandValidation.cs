using FluentValidation;
using System.Application.Services.Progress.StudentStage.Command;

namespace System.Application.Services.Progress.StudentStage.Validations
{
    public sealed class NewStudentStageCommandValidation : AbstractValidator<NewStudentStageCommand>
    {
        public NewStudentStageCommandValidation()
        {
            RuleFor(command => command.StudentAcademicEnrollmentID).Must(IsGuid)
                .WithMessage("StudentAcademicEnrollmentID must be a valid GUID");
            RuleFor(command => command.StageID).Must(IsGuid)
                .WithMessage("StageID must be a valid GUID");
            RuleFor(command => command.StudentStageState).IsInEnum();
            RuleFor(command => command.StudentStageDate).NotEqual(default(DateOnly));
        }

        private static bool IsGuid(string? value) => Guid.TryParse(value, out var id) && id != Guid.Empty;
    }
}
