using FluentValidation;
using System;
using System.Application.Services.Address.Country.Command;
using System.Application.Services.Address.District.Command;
using System.Application.Services.Address.Neighborhood.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Address.Neighborhood.Validation
{
    public class NewNeighborhoodCommandValidator : AbstractValidator<NewNeighborhoodCommand>
    {
        public NewNeighborhoodCommandValidator()
        {
            RuleFor(x => x.NeighborhoodName).NotEmpty().WithMessage("Neighborhood Name MustBe Not Empty");
            RuleFor(x => x.DistrictID).NotEmpty().WithMessage("District ID MustBe Not Empty");
        }
    }
}
