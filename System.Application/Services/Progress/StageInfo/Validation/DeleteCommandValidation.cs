using FluentValidation;
using System.Application.Services.Progress.StageInfo.Command;


namespace System.Application.Services.Progress.StageInfo.Validator
{
    public class DeleteStageInfoCommandValidation : AbstractValidator<DeleteStageInfoCommand>
    {
        public DeleteStageInfoCommandValidation()
        {
            RuleFor(x => x.StageInfoID).NotEmpty().WithMessage("StageInfo id MustBe Not Empty");
        }
    }
}
