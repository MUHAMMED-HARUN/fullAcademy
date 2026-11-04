using FluentValidation;
using System.Application.Services.Address.Country.Command;

namespace System.Application.Services.Address.Country.Validations
{
    public class NewCountryCommandValidator:AbstractValidator<NewCountryCommand>
    {
        public NewCountryCommandValidator()
        {
            RuleFor(x => x.CountryName)
                .NotEmpty().WithMessage("اسم الدولة مطلوب")
                .MinimumLength(2).WithMessage("اسم الدولة يجب أن يكون حرفين على الأقل")
                .MaximumLength(100).WithMessage("اسم الدولة طويل جداً");
            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("رمز الدولة مطلوب")
                .MaximumLength(10).WithMessage("رمز الدولة طويل جداً");
        }
    }
}
