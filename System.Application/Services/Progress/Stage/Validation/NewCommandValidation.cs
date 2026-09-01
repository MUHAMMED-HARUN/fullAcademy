using FluentValidation;
using System.Application.Services.Progress.Stage.Command;


namespace System.Application.Services.Progress.Stage.Validator
{
    public class NewStageCommandValidation : AbstractValidator<NewStageCommand>
    {

        public NewStageCommandValidation()
        {
            RuleFor(x => x.StageName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.StageNote).NotEmpty().WithMessage("Note MustBe Not Empty");
            RuleFor(x => x.ProgramID).NotEmpty().WithMessage("Note MustBe Not Empty");
        }
    }

}
