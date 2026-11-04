using FluentValidation;
using System.Application.Services.Progress.Stage.Command;


namespace System.Application.Services.Progress.Stage.Validations
{
    public class DeleteStageCommandValidation : AbstractValidator<DeleteStageCommand>
    {
        public DeleteStageCommandValidation()
        {
            RuleFor(x => x.StageID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("StageID must be a valid, non-empty GUID");
        }
    }
}
