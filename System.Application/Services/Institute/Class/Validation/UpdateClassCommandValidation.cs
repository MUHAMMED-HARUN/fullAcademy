using FluentValidation;
using System.Application.Services.Institute.Class.Command;

namespace System.Application.Services.Institute.Class.Validation
{
    public class UpdateClassCommandValidation : AbstractValidator<UpdateClassCommand>
    {
        public UpdateClassCommandValidation()
        {
            RuleFor(x => x.ClassID).NotEmpty().WithMessage("id MustBe Not Empty");
            RuleFor(x => x.ClassName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.ClassNotes).NotEmpty().WithMessage("Note MustBe Not Empty");
            RuleFor(x => x.ClassLevel).GreaterThan(0).WithMessage("Level Greter than 0").NotNull().WithMessage("Level MustBe Not Empty");
            RuleFor(x => x.ProgramID).NotEmpty().WithMessage("Note MustBe Not Empty");

        }
    }
}
