using FluentValidation;
using System.Application.Services.Finance.StudentInvoice.Command;

namespace System.Application.Services.Finance.StudentInvoice.Validation
{
    public class NewStudentInvoiceCommandValidation : AbstractValidator<NewStudentInvoiceCommand>
    {
        public NewStudentInvoiceCommandValidation()
        {
            RuleFor(x => x.StudentID)
                .NotEmpty().WithMessage("StudentID is required");

            RuleFor(x => x.FeeStructureID)
                .NotEmpty().WithMessage("FeeStructureID is required");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("DueDate is required");

            RuleFor(x => x.OriginalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("OriginalAmount must be greater than or equal to 0");

            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("DiscountAmount must be greater than or equal to 0");
        }
    }

    public class UpdateStudentInvoiceCommandValidation : AbstractValidator<UpdateStudentInvoiceCommand>
    {
        public UpdateStudentInvoiceCommandValidation()
        {
            RuleFor(x => x.StudentInvoiceID)
                .NotEmpty().WithMessage("StudentInvoiceID is required");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("DueDate is required");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid enum value");
        }
    }

    public class DeleteStudentInvoiceCommandValidation : AbstractValidator<DeleteStudentInvoiceCommand>
    {
        public DeleteStudentInvoiceCommandValidation()
        {
            RuleFor(x => x.StudentInvoiceID)
                .NotEmpty().WithMessage("StudentInvoiceID is required");
        }
    }
}
