

using FluentValidation;

namespace System.Application.Services.Authentication.Login.Command
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
    public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("User Name MustBe Not Empty");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password Must Be greater Than 8");
        }
    }
}
