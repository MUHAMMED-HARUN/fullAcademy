using FluentValidation;
using System.Application.Services.Institute.Class.Command;


namespace System.Application.Services.Institute.Class.Validator
{
    public class DeleteClassCommandValidation : AbstractValidator<DeleteClassCommand>
    {
        public DeleteClassCommandValidation()
        {
            RuleFor(x => x.ClassID).NotEmpty().WithMessage("Class id MustBe Not Empty");
        }
    }
}
