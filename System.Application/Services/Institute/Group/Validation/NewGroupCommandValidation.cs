using FluentValidation;
using System.Application.Services.Institute.Group.Command;

namespace System.Application.Services.Institute.Group.Validation
{
    public class NewGroupCommandValidation : AbstractValidator<NewGroupCommand>
    {
        public NewGroupCommandValidation()
        {
            RuleFor(x => x.GroupName)
                .NotEmpty().WithMessage("GroupName must not be empty")
                .MaximumLength(150).WithMessage("GroupName must not exceed 150 characters");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code must not be empty")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");
            RuleFor(x => x.InstituteClassID)
                .Must(IsGuid).WithMessage("InstituteClassID must be a valid GUID");
        }

        private static bool IsGuid(string? value) => Guid.TryParse(value, out var id) && id != Guid.Empty;
    }
}
