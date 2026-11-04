using FluentValidation;
using System.Application.Services.Finance.Payment.Command;

namespace System.Application.Services.Finance.Payment.Validation
{
    public class NewPaymentCommandValidation : AbstractValidator<NewPaymentCommand>
    {
        public NewPaymentCommandValidation()
        {
            RuleFor(x => x.StudentInvoiceID)
                .NotEmpty().WithMessage("StudentInvoiceID is required");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("PaymentMethod must be a valid enum value");
        }
    }

    public class UpdatePaymentCommandValidation : AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentCommandValidation()
        {
            RuleFor(x => x.PaymentID)
                .NotEmpty().WithMessage("PaymentID is required");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("PaymentMethod must be a valid enum value");
        }
    }

    public class DeletePaymentCommandValidation : AbstractValidator<DeletePaymentCommand>
    {
        public DeletePaymentCommandValidation()
        {
            RuleFor(x => x.PaymentID)
                .NotEmpty().WithMessage("PaymentID is required");
        }
    }
}
