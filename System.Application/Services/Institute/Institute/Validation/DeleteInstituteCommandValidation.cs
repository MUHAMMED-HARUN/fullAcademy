using FluentValidation;
using System.Application.Services.Institute.Institute.Command;


namespace System.Application.Services.Institute.Institute.Validator
{
    public class DeleteInstituteCommandValidation : AbstractValidator<DeleteInstituteCommand>
    {
        public DeleteInstituteCommandValidation()
        {
            RuleFor(x => x.InstituteID).NotEmpty().WithMessage("Institute id MustBe Not Empty");
        }
    }
}
