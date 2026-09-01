using FluentValidation;
using System;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Institute.Institute.Command;

namespace System.Application.Services.Person.Person.Validation
{
    public class UpdateInstituteCommandValidation : AbstractValidator<UpdateInstituteCommand>
    {
        public UpdateInstituteCommandValidation()
        {
            RuleFor(x => x.InstituteID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
            RuleFor(x => x.InstituteNotes).NotEmpty().WithMessage("First Name MustBe Not Empty");
            RuleFor(x => x.InstituteName).NotEmpty().WithMessage("Father Name MustBe Not Empty");

            RuleFor<UpdateAddressCommand>(x => x.Address); // Testing

        }
    }
}
