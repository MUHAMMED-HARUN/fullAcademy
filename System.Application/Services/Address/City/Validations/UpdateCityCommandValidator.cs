using FluentValidation;
using System.Application.Services.Address.City.Command;

namespace System.Application.Services.Address.City.Validations
{
    public class UpdateCityCommandValidator:AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(x => x.CityID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("معرّف المدينة غير صالح");
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
