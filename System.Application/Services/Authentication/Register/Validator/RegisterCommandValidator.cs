
using FluentValidation;
using System.Application.Services.Authentication.Register.Command;

namespace System.Application.Services.Authentication.Register.Validator
{
    public class RegisterCommandValidator: AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(4);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
                .Matches(@"[A-Z]").WithMessage("يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل.");
        }
    }
}
