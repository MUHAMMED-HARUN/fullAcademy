using FluentValidation;
using System.Application.Services.Institute.Group.Command;

namespace System.Application.Services.Institute.Group.Validation
{
    public class NewGroupCommandValidation : AbstractValidator<NewGroupCommand>
    {
        public NewGroupCommandValidation()
        {
            RuleFor(x => x.GroupName).NotEmpty().WithMessage("GroupName must not be empty");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code must not be empty");
            RuleFor(x => x.InstituteClassID).NotEmpty().WithMessage("InstituteClassID must not be empty");
        }
    }
}
