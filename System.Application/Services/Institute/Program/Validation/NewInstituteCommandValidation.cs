using FluentValidation;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Program.Command;

namespace System.Application.Services.Institute.Institute.Validator
{
    public class NewProgramCommandValidation : AbstractValidator<NewProgramCommand>
    {

        public NewProgramCommandValidation()
        {
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.ProgramNote).NotEmpty().WithMessage("Note MustBe Not Empty");
        }
    }

}
