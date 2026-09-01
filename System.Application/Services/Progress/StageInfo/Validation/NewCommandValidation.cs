using FluentValidation;
using System.Application.Services.Progress.StageInfo.Command;


namespace System.Application.Services.Progress.StageInfo.Validator
{
    public class NewStageInfoCommandValidation : AbstractValidator<NewStageInfoCommand>
    {

        public NewStageInfoCommandValidation()
        {
            RuleFor(x => x.From).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("From MustBe Not Empty");
            RuleFor(x => x.To).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("to MustBe Not Empty");
            RuleFor(x => x.MatterID).NotEmpty().WithMessage("Matter MustBe Not Empty");
            RuleFor(x => x.StageID).NotEmpty().WithMessage("StageID MustBe Not Empty");
            RuleFor(x => x.UniteType).IsInEnum().WithMessage("UniteType MustBe Not Empty");
            RuleFor(x => x.ProgressType).IsInEnum().WithMessage("ProgressType MustBe Not Empty");
        }
    }

}
