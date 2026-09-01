
using FluentValidation;
using System.Application.Services.Authentication.Login.Command;

namespace System.Application.Services.Authentication.Login.Validator
{
    public class LoginCommandValidator: AbstractValidator<LoginCommand>
    {

        public LoginCommandValidator()
        {
            RuleFor(x=>x.Email).NotEmpty().MinimumLength(4);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).
                Matches(@"[A-Z]").WithMessage("يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل."); ;
        }
    }
}
