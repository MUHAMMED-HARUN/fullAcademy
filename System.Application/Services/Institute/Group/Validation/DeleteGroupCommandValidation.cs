using FluentValidation;
using System.Application.Services.Institute.Group.Command;

namespace System.Application.Services.Institute.Group.Validation
{
    public class DeleteGroupCommandValidation : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidation()
        {
            RuleFor(x => x.GroupID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("GroupID must be a valid GUID");
        }
    }
}
