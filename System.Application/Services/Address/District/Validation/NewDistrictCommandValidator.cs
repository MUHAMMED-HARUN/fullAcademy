using FluentValidation;
using System;
using System.Application.Services.Address.Country.Command;
using System.Application.Services.Address.District.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Address.District.Validation
{
    public class NewDistrictCommandValidator : AbstractValidator<NewDistrictCommand>
    {
        public NewDistrictCommandValidator()
        {
            RuleFor(x => x.DistrictName).NotEmpty().WithMessage("District Name MustBe Not Empty");
            RuleFor(x => x.CityID).NotEmpty().WithMessage("City ID MustBe Not Empty");
        }
    }
}
