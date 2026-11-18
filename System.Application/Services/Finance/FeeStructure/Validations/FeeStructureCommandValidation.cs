using FluentValidation;
using System.Application.Services.Finance.FeeStructure.Command;

namespace System.Application.Services.Finance.FeeStructure.Validations
{
    public class NewFeeStructureCommandValidation : AbstractValidator<NewFeeStructureCommand>
    {
        public NewFeeStructureCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.FeeType)
                .IsInEnum().WithMessage("FeeType must be a valid enum value");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");

            RuleFor(x => x.AcademicPlanID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AcademicPlanID must be a valid identifier");
        }
    }

    public class UpdateFeeStructureCommandValidation : AbstractValidator<UpdateFeeStructureCommand>
    {
        public UpdateFeeStructureCommandValidation()
        {
            RuleFor(x => x.FeeStructureID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("FeeStructureID must be a valid identifier");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.FeeType)
                .IsInEnum().WithMessage("FeeType must be a valid enum value");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");

            RuleFor(x => x.AcademicPlanID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AcademicPlanID must be a valid identifier");
        }
    }

    public class DeleteFeeStructureCommandValidation : AbstractValidator<DeleteFeeStructureCommand>
    {
        public DeleteFeeStructureCommandValidation()
        {
            RuleFor(x => x.FeeStructureID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("FeeStructureID must be a valid identifier");
        }
    }

}
