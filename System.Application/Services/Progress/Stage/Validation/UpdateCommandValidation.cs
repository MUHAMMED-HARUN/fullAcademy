using FluentValidation;
using System.Application.Services.Progress.Stage.Command;


namespace System.Application.Services.Person.Person.Validation
{
    public class UpdateStageCommandValidation : AbstractValidator<UpdateStageCommand>
    {
        public UpdateStageCommandValidation()
        {
            RuleFor(x => x.StageID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
            RuleFor(x => x.StageName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.ProgramID).NotEmpty().WithMessage("Note MustBe Not Empty");
        }
    }
}
