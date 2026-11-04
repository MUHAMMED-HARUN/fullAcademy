using FluentValidation;
using System.Application.Services.Authentication.Login.Command;

namespace System.Application.Services.Authentication.Login.Validations
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب")
                .EmailAddress().WithMessage("صيغة البريد الإلكتروني غير صحيحة");
            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة");
        }
    }
}
