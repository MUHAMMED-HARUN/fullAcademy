using FluentValidation;
using System.Application.Services.Finance.StudentInvoice.Command;

namespace System.Application.Services.Finance.StudentInvoice.Validations
{
    public class GenerateInvoicesForEnrollmentCommandValidation : AbstractValidator<GenerateInvoicesForEnrollmentCommand>
    {
        public GenerateInvoicesForEnrollmentCommandValidation()
        {
            RuleFor(x => x.StudentAcademicEnrollmentID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID must be a valid identifier");

            RuleFor(x => x.FeeStructureID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("FeeStructureID must be a valid identifier");

            RuleFor(x => x.BillingPeriodStart).NotEmpty();
            RuleFor(x => x.BillingPeriodEnd)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.BillingPeriodStart);
        }
    }

    public class UpdateStudentInvoiceCommandValidation : AbstractValidator<UpdateStudentInvoiceCommand>
    {
        public UpdateStudentInvoiceCommandValidation()
        {
            RuleFor(x => x.StudentInvoiceID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentInvoiceID must be a valid identifier");

            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("DiscountAmount must be greater than or equal to 0");
        }
    }

    public class DeleteStudentInvoiceCommandValidation : AbstractValidator<DeleteStudentInvoiceCommand>
    {
        public DeleteStudentInvoiceCommandValidation()
        {
            RuleFor(x => x.StudentInvoiceID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentInvoiceID must be a valid identifier");
        }
    }
}
