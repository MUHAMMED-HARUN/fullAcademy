using FluentValidation;
using System.Application.Services.Progress.StudentStage.Command;


namespace System.Application.Services.Progress.StudentStage.Validator
{
    public class NewStudentStageCommandValidation : AbstractValidator<NewStudentStageCommand>
    {

        public NewStudentStageCommandValidation()
        {
            RuleFor(x => x.EnrollmentStudentID).NotEmpty().WithMessage("Enrollment Student ID MustBe Not Empty");
            RuleFor(x => x.StageID).NotEmpty().WithMessage("Stage ID MustBe Not Empty");
            RuleFor(x => x.StudentStageState).IsInEnum().WithMessage("State MustBe Not Empty");
        }
    }

}
