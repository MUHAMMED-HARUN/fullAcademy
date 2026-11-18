using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.Command
{
    public class NewPaymentCommandHandler : IRequestHandler<NewPaymentCommand, TResult<string>>
    {
        private readonly IPaymentRepository _repo;
        private readonly IStudentInvoiceRepository _invoiceRepo;
        private readonly ICurrentUserService _currentUserService;

        public NewPaymentCommandHandler(
            IPaymentRepository repo,
            IStudentInvoiceRepository invoiceRepo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _invoiceRepo = invoiceRepo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(NewPaymentCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentInvoiceID, out var invoiceID) || invoiceID == Guid.Empty)
                return TResult<string>.Fail("StudentInvoiceID must be a valid identifier.");
            var invoice = await _invoiceRepo.GetEntityForPayment(invoiceID);
            if (invoice == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);
            if (invoice.Status == enInvoiceStatus.Cancelled)
                return TResult<string>.Fail(ApplicationExMsg.InvoiceCancelled);
            if (request.Amount > invoice.RemainingAmount)
                return TResult<string>.Fail(ApplicationExMsg.PaymentExceedsRemaining);

            if (request.Amount <= 0 || request.Amount > invoice.RemainingAmount)
                return TResult<string>.Fail(ApplicationExMsg.PaymentExceedsRemaining);

            var entity = new Domain.Entities.Finance.clsPayment(
                invoiceID,
                request.Amount,
                request.PaymentDate,
                request.PaymentMethod,
                request.ReferenceNumber,
                request.Notes);
            var userID = _currentUserService.UserId?.ToString() ?? "System";
            entity.SetCreated(userID);
            invoice.ApplyPayment(request.Amount, DateOnly.FromDateTime(entity.PaymentDate));
            invoice.SetModified(userID);

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            await _invoiceRepo.Update(invoice);

            return TResult<string>.OK(entityID);
        }
    }
}
