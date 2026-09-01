using FluentValidation;
using System.Application.Services.Progress.StudentStage.Command;


namespace System.Application.Services.Progress.StudentStage.Validator
{
    public class DeleteStudentStageCommandValidation : AbstractValidator<DeleteStudentStageCommand>
    {
        public DeleteStudentStageCommandValidation()
        {
            RuleFor(x => x.StudentStageID).NotEmpty().WithMessage("StudentStage id MustBe Not Empty");
        }
    }
}
