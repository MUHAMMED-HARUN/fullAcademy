using FluentValidation;
using System.Application.Services.Address.District.Command;
using System.Application.Services.Address.Neighborhood.Command;


namespace System.Application.Services.Address.Neighborhood.Validation
{
    public class UpdateNeighborhoodCommandValidator : AbstractValidator<UpdateNeighborhoodCommand>
    {
        public UpdateNeighborhoodCommandValidator()
        {
            RuleFor(x => x.NeighborhoodName).NotEmpty().WithMessage("Neighborhood Name MustBe Not Empty");
            RuleFor(x => x.DistrictID).NotEmpty().WithMessage("District ID MustBe Not Empty");
        }
    }
}
