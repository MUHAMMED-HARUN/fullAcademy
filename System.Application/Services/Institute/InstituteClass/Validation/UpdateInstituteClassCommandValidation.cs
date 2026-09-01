using FluentValidation;
using System;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.InstituteClass.Command;
using System.Application.Services.Institute.Program.Command;

namespace System.Application.Services.Person.Person.Validation
{
    public class UpdateInstituteClassCommandValidation : AbstractValidator<UpdateInstituteClassCommand>
    {
        public UpdateInstituteClassCommandValidation()
        {
            RuleFor(x => x.InstituteClassID).NotEmpty().WithMessage("InstituteClassID MustBe Not Empty");
            RuleFor(x => x.InstituteID).NotEmpty().WithMessage("InstituteID MustBe Not Empty");
            RuleFor(x => x.ClassID).NotEmpty().WithMessage("ClassID MustBe Not Empty");
        }
    }
}
