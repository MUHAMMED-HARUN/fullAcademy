using FluentValidation;
using System.Application.Services.Institute.Group.Command;

namespace System.Application.Services.Institute.Group.Validation
{
    public class DeleteGroupCommandValidation : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidation()
        {
            RuleFor(x => x.GroupID).NotEmpty().WithMessage("GroupID must not be empty");
        }
    }
}
