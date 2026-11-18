using FluentValidation;
using System.Application.Services.Progress.StageInfo.Command;


namespace System.Application.Services.Progress.StageInfo.Validations
{
    public class DeleteStageInfoCommandValidation : AbstractValidator<DeleteStageInfoCommand>
    {
        public DeleteStageInfoCommandValidation()
        {
            RuleFor(x => x.StageInfoID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StageInfoID must be a valid, non-empty GUID");
        }
    }
}
