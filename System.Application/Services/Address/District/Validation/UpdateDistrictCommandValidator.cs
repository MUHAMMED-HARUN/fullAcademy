using FluentValidation;
using System.Application.Services.Address.District.Command;


namespace System.Application.Services.Address.District.Validation
{
    public class UpdateDistrictCommandValidator : AbstractValidator<UpdateDistrictCommand>
    {
        public UpdateDistrictCommandValidator()
        {
            RuleFor(x => x.DistrictName).NotEmpty().WithMessage("District Name MustBe Not Empty");
            RuleFor(x => x.CityID).NotEmpty().WithMessage("City ID MustBe Not Empty");
        }
    }
}
