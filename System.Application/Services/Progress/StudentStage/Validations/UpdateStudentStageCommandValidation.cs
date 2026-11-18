using FluentValidation;
using System.Application.Services.Progress.StudentStage.Command;

namespace System.Application.Services.Progress.StudentStage.Validations
{
    public sealed class UpdateStudentStageCommandValidation : AbstractValidator<UpdateStudentStageCommand>
    {
        public UpdateStudentStageCommandValidation()
        {
            RuleFor(command => command.StudentStageID).Must(IsGuid)
                .WithMessage("StudentStageID must be a valid GUID");
            RuleFor(command => command.StudentStageState).IsInEnum();
            RuleFor(command => command.StudentStageDate).NotEqual(default(DateOnly));
        }

        private static bool IsGuid(string? value) => Guid.TryParse(value, out var id) && id != Guid.Empty;
    }
}
