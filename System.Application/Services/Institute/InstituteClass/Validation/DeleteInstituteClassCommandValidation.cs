using FluentValidation;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.InstituteClass.Command;


namespace System.Application.Services.Institute.Institute.Validator
{
    public class DeleteInstituteClassCommandValidation : AbstractValidator<DeleteInstituteClassCommand>
    {
        public DeleteInstituteClassCommandValidation()
        {
            RuleFor(x => x.InstituteClassID).NotEmpty().WithMessage("InstituteClass id MustBe Not Empty");
        }
    }
}
