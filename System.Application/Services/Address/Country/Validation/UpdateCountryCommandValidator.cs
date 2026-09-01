using FluentValidation;
using System;
using System.Application.Services.Address.Country.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Address.Country.Validation
{
    public class UpdateCountryCommandValidator:AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator()
        {
            RuleFor(x => x.CountryID).NotEmpty().WithMessage("Country id Code MustBe Not Empty");
            RuleFor(x => x.CountryCode).NotEmpty().WithMessage("Country Code MustBe Not Empty");
            RuleFor(x => x.CountryName).NotEmpty().WithMessage("Country Name MustBe Not Empty");
        }
    }
}
