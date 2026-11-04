using FluentValidation;
using System.Application.Services.Address.City.Command;

namespace System.Application.Services.Address.City.Validations
{
    public class NewCityCommandValidator:AbstractValidator<NewCityCommand>
    {
        public NewCityCommandValidator()
        {
            RuleFor(x => x.CityName)
                .NotEmpty().WithMessage("اسم المدينة مطلوب")
                .MinimumLength(2).WithMessage("اسم المدينة يجب أن يكون حرفين على الأقل")
                .MaximumLength(100).WithMessage("اسم المدينة طويل جداً");
            RuleFor(x => x.CityCode)
                .MaximumLength(10).WithMessage("رمز المدينة طويل جداً");
            RuleFor(x => x.CountryID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("معرّف الدولة غير صالح");
        }
    }
}
