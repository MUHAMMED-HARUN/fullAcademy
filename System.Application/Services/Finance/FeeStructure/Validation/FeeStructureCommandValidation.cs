using FluentValidation;
using System.Application.Services.Finance.FeeStructure.Command;

namespace System.Application.Services.Finance.FeeStructure.Validation
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
        }
    }

    public class UpdateFeeStructureCommandValidation : AbstractValidator<UpdateFeeStructureCommand>
    {
        public UpdateFeeStructureCommandValidation()
        {
            RuleFor(x => x.FeeStructureID)
                .NotEmpty().WithMessage("FeeStructureID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.FeeType)
                .IsInEnum().WithMessage("FeeType must be a valid enum value");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");
        }
    }

    public class DeleteFeeStructureCommandValidation : AbstractValidator<DeleteFeeStructureCommand>
    {
        public DeleteFeeStructureCommandValidation()
        {
            RuleFor(x => x.FeeStructureID)
                .NotEmpty().WithMessage("FeeStructureID is required");
        }
    }
}
