using FluentValidation;
using System.Application.Services.Institute.Institute.Command;


namespace System.Application.Services.Institute.Institute.Validator
{
    public class DeleteProgramCommandValidation : AbstractValidator<DeleteProgramCommand>
    {
        public DeleteProgramCommandValidation()
        {
            RuleFor(x => x.ProgramID).NotEmpty().WithMessage("Program id MustBe Not Empty");
        }
    }
}
