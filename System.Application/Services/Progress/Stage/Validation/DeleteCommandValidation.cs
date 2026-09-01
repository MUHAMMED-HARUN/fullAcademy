using FluentValidation;
using System.Application.Services.Progress.Stage.Command;


namespace System.Application.Services.Progress.Stage.Validator
{
    public class DeleteStageCommandValidation : AbstractValidator<DeleteStageCommand>
    {
        public DeleteStageCommandValidation()
        {
            RuleFor(x => x.StageID).NotEmpty().WithMessage("Stage id MustBe Not Empty");
        }
    }
}
