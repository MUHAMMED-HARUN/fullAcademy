using FluentValidation;
using System.Application.Services.Assignment.Result.Command;

namespace System.Application.Services.Assignment.Result.Validations
{
    public sealed class DeleteResultCommandValidation : AbstractValidator<DeleteResultCommand>
    {
        public DeleteResultCommandValidation()
        {
            RuleFor(command => command.ResultID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("ResultID must be a valid GUID");
        }
    }
}
