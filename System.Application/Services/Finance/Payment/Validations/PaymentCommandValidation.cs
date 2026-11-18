using FluentValidation;
using System.Application.Services.Finance.Payment.Command;

namespace System.Application.Services.Finance.Payment.Validations
{
    public class NewPaymentCommandValidation : AbstractValidator<NewPaymentCommand>
    {
        public NewPaymentCommandValidation()
        {
            RuleFor(x => x.StudentInvoiceID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentInvoiceID must be a valid identifier");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("PaymentMethod must be a valid enum value");
        }
    }

    public class ReversePaymentCommandValidation : AbstractValidator<ReversePaymentCommand>
    {
        public ReversePaymentCommandValidation()
        {
            RuleFor(x => x.PaymentID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("PaymentID must be a valid identifier");

            RuleFor(x => x.Reason)
                .NotEmpty().MaximumLength(500)
                .WithMessage("A reversal reason of at most 500 characters is required.");
        }
    }
}
