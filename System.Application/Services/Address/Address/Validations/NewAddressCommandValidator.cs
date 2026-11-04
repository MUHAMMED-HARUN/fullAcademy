using FluentValidation;
using System.Application.Services.Address.Address.Command;

namespace System.Application.Services.Address.Address.Validations
{
    public class NewAddressCommandValidator : AbstractValidator<NewAddressCommand>
    {
        public NewAddressCommandValidator()
        {
            RuleFor(x => x.CountryID).Must(BeValidGuid).WithMessage("معرّف الدولة غير صالح");
            RuleFor(x => x.CityID).Must(BeValidGuid).WithMessage("معرّف المدينة غير صالح");
            RuleFor(x => x.DistrictID).Must(BeValidGuid).WithMessage("معرّف المنطقة غير صالح");
            RuleFor(x => x.NeighborhoodID).Must(BeValidGuid).WithMessage("معرّف الشارع غير صالح");
            RuleFor(x => x.Notes)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage("ملاحظات العنوان مطلوبة")
                .MaximumLength(100).WithMessage("ملاحظات العنوان طويلة جداً");
        }

        private static bool BeValidGuid(string value) =>
            Guid.TryParse(value, out var id) && id != Guid.Empty;
    }
}
