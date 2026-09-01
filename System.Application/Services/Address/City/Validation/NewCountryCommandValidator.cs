using FluentValidation;
using System;
using System.Application.Services.Address.City.Command;
using System.Application.Services.Address.Country.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Address.City.Validation
{
    public class NewCityCommandValidator:AbstractValidator<NewCityCommand>
    {
        public NewCityCommandValidator()
        {
            RuleFor(x => x.CityCode).NotEmpty().WithMessage("City Code MustBe Not Empty");
            RuleFor(x => x.CityName).NotEmpty().WithMessage("City Name MustBe Not Empty");
            RuleFor(x => x.CountryID).NotEmpty().WithMessage("Country ID MustBe Not Empty");
        }
    }
}
