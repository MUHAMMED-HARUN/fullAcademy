using FluentValidation;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.InstituteClass.Command;
using System.Application.Services.Institute.Program.Command;

namespace System.Application.Services.Institute.Institute.Validator
{
    public class NewInstituteClassCommandValidation : AbstractValidator<NewInstituteClassCommand>
    {

        public NewInstituteClassCommandValidation()
        {
            RuleFor(x => x.InstituteID).NotEmpty().WithMessage("InstituteID MustBe Not Empty");
            RuleFor(x => x.ClassID).NotEmpty().WithMessage("ClassID MustBe Not Empty");
        }
    }

}
