using FluentValidation;
using System.Application.Services.Progress.StageInfo.Command;


namespace System.Application.Services.Progress.StageInfo.Validations
{
    public class NewStageInfoCommandValidation : AbstractValidator<NewStageInfoCommand>
    {

        public NewStageInfoCommandValidation()
        {
            RuleFor(x => x.From).GreaterThanOrEqualTo(0);
            RuleFor(x => x.To).GreaterThanOrEqualTo(x => x.From);
            RuleFor(x => x.MatterID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("MatterID must be a valid, non-empty GUID");
            RuleFor(x => x.StageID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StageID must be a valid, non-empty GUID");
            RuleFor(x => x.UniteType).IsInEnum();
            RuleFor(x => x.ProgressType).IsInEnum();
        }
    }

}
