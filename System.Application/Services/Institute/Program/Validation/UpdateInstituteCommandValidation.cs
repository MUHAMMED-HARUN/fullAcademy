using FluentValidation;
using System;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Program.Command;

namespace System.Application.Services.Person.Person.Validation
{
    public class UpdateProgramCommandValidation : AbstractValidator<UpdateProgramCommand>
    {
        public UpdateProgramCommandValidation()
        {
            RuleFor(x => x.ProgramID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.ProgramNote).NotEmpty().WithMessage("Note MustBe Not Empty");
        }
    }
}
