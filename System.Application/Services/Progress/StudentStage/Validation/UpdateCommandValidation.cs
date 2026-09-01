using FluentValidation;
using System.Application.Services.Progress.StudentStage.Command;


namespace System.Application.Services.Person.Person.Validation
{
    public class UpdateStudentStageCommandValidation : AbstractValidator<UpdateStudentStageCommand>
    {
        public UpdateStudentStageCommandValidation()
        {
            RuleFor(x => x.StudentStageID).NotEmpty().WithMessage("StudentStage ID MustBe Not Empty");
            RuleFor(x => x.EnrollmentStudentID).NotEmpty().WithMessage("Enrollment Student ID MustBe Not Empty");
            RuleFor(x => x.StageID).NotEmpty().WithMessage("Stage ID MustBe Not Empty");
            RuleFor(x => x.StudentStageState).IsInEnum().WithMessage("State MustBe Not Empty");
        }
    }
}
