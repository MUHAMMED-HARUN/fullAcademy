using FluentValidation;
using System.Application.Services.Progress.Stage.Command;


namespace System.Application.Services.Progress.Stage.Validations
{
    public class NewStageCommandValidation : AbstractValidator<NewStageCommand>
    {

        public NewStageCommandValidation()
        {
            RuleFor(x => x.StageName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StageNote).MaximumLength(500);
            RuleFor(x => x.ClassID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("ClassID must be a valid, non-empty GUID");
            RuleFor(x => x.PrevStageID)
                .Must(value => string.IsNullOrWhiteSpace(value) || Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("PrevStageID must be a valid GUID");
        }
    }

}
