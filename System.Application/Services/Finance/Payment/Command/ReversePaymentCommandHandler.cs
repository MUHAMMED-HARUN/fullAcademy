using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.Command
{
    public sealed class ReversePaymentCommandHandler : IRequestHandler<ReversePaymentCommand, TResult<string>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IStudentInvoiceRepository _invoiceRepository;
        private readonly ICurrentUserService _currentUser;

        public ReversePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IStudentInvoiceRepository invoiceRepository,
            ICurrentUserService currentUser)
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _currentUser = currentUser;
        }

        public async Task<TResult<string>> Handle(ReversePaymentCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.PaymentID, out var paymentID) || paymentID == Guid.Empty)
                return TResult<string>.Fail("PaymentID must be a valid identifier.");
            if (string.IsNullOrWhiteSpace(request.Reason))
                return TResult<string>.Fail("A reversal reason is required.");
            if (_currentUser.UserId is not Guid userID)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede ?? "The current user cannot reverse payments.");

            var payment = await _paymentRepository.GetPaymentForReversal(paymentID);
            if (payment == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);
            if (await _paymentRepository.HasReversal(paymentID))
                return TResult<string>.Fail("This payment has already been reversed.");

            var invoice = await _invoiceRepository.GetEntityForPayment(payment.StudentInvoiceID);
            if (invoice == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            invoice.ReversePayment(payment.Amount);
            invoice.SetModified(userID.ToString());
            var reversal = new clsPaymentReversal(paymentID, payment.Amount, request.Reason, userID.ToString());

            await _paymentRepository.AddReversal(reversal);
            await _invoiceRepository.Update(invoice);

            return TResult<string>.OK(reversal.Id.ToString());
        }
    }
}
