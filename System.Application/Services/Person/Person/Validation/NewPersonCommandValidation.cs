using FluentValidation;
using System;
using System.Application.Services.Person.Person.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Person.Person.Validation
{
    public class NewPersonCommandValidation : AbstractValidator<NewPersonCommand>
    {

        public NewPersonCommandValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name MustBe Not Empty");
            RuleFor(x => x.FatherName).NotEmpty().WithMessage("Father Name MustBe Not Empty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name MustBe Not Empty");
            RuleFor(x => x.MotherName).NotEmpty().WithMessage("Mothet Name MustBe Not Empty");
            RuleFor(x => x.MotherLastName).NotEmpty().WithMessage("Mother Last MustBe Not Empty");
            RuleFor(x => x.NationalNumber).NotEmpty().WithMessage("National Number MustBe Not Empty");
            RuleFor(x => x.BirthDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).NotEmpty().WithMessage("Birth Date MustBe Less Than Now");
            RuleFor(x => x.PlaceOfBirthID).NotEmpty().WithMessage("Place Of Birth MustBe Not Empty");
        }
    }

}
