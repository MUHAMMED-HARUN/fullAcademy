using FluentValidation;
using System.Application.Services.Address.District.Command;


namespace System.Application.Services.Address.District.Validations
{
    public class UpdateDistrictCommandValidator : AbstractValidator<UpdateDistrictCommand>
    {
        public UpdateDistrictCommandValidator()
        {
            RuleFor(x => x.DistrictID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("معرّف المنطقة غير صالح");
            RuleFor(x => x.DistrictName)
                .NotEmpty().WithMessage("اسم المنطقة مطلوب")
                .MinimumLength(2).WithMessage("اسم المنطقة يجب أن يكون حرفين على الأقل")
                .MaximumLength(100).WithMessage("اسم المنطقة طويلة جداً");
            RuleFor(x => x.CityID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("معرّف المدينة غير صالح");
        }
    }
}
