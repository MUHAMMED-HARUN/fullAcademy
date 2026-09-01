using FluentValidation;
using System;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Address.City.Command;
using System.Application.Services.Address.Country.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Address.Address.Validation
{
    public class NewAddressCommandValidator : AbstractValidator<NewAddressCommand>
    {
        public NewAddressCommandValidator()
        {
            RuleFor(x => x.CountryID).NotEmpty().WithMessage("Country ID MustBe Not Empty");
            RuleFor(x => x.CityID).NotEmpty().WithMessage("City ID MustBe Not Empty");
            RuleFor(x => x.DistrictID).NotEmpty().WithMessage("District ID MustBe Not Empty");
            RuleFor(x => x.NeighborhoodID).NotEmpty().WithMessage("Neighborhood ID MustBe Not Empty");
            RuleFor(x => x.Notes).NotEmpty().WithMessage("Notes MustBe Not Empty");
        }
    }
}
