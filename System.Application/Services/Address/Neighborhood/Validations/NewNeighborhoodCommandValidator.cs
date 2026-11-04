using FluentValidation;
using System.Application.Services.Address.Neighborhood.Command;

namespace System.Application.Services.Address.Neighborhood.Validations
{
    public class NewNeighborhoodCommandValidator : AbstractValidator<NewNeighborhoodCommand>
    {
        public NewNeighborhoodCommandValidator()
        {
            RuleFor(x => x.NeighborhoodName)
                .NotEmpty().WithMessage("اسم الشارع مطلوب")
                .MinimumLength(2).WithMessage("اسم الشارع يجب أن يكون حرفين على الأقل")
                .MaximumLength(100).WithMessage("اسم الشارع طويل جداً");
            RuleFor(x => x.DistrictID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("معرّف المنطقة غير صالح");
        }
    }
}
