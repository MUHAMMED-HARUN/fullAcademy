using FluentValidation;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Institute.Institute.Command;

namespace System.Application.Services.Institute.Institute.Validator
{
    public class NewInstituteCommandValidation : AbstractValidator<NewInstituteCommand>
    {

        public NewInstituteCommandValidation()
        {
            RuleFor(x => x.InstituteName).NotEmpty().WithMessage("First Name MustBe Not Empty");
            RuleFor(x => x.InstituteNotes).NotEmpty().WithMessage("Father Name MustBe Not Empty");

            RuleFor<NewAddressCommand>(x => x.Address);// testing
        }
    }

}
