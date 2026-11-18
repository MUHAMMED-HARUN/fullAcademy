using FluentValidation;
using System.Application.Services.Progress.StudentStage.Command;

namespace System.Application.Services.Progress.StudentStage.Validations
{
    public sealed class DeleteStudentStageCommandValidation : AbstractValidator<DeleteStudentStageCommand>
    {
        public DeleteStudentStageCommandValidation()
        {
            RuleFor(command => command.StudentStageID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentStageID must be a valid GUID");
        }
    }
}
