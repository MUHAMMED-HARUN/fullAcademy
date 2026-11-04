using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.Command
{
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, TResult<string>>
    {
        private readonly IPaymentRepository _repo;
        private readonly IStudentInvoiceRepository _invoiceRepo;
        private readonly ICurrentUserService _currentUserService;

        public DeletePaymentCommandHandler(
            IPaymentRepository repo,
            IStudentInvoiceRepository invoiceRepo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _invoiceRepo = invoiceRepo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(request.PaymentID, enPaymentParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            var invoice = await _invoiceRepo.GetEntityBy(entity.StudentInvoiceID.ToString(), enStudentInvoiceParamType.ID);
            if (invoice != null)
            {
                invoice.ReversePayment(entity.Amount);
                invoice.SetModified(_currentUserService.UserId.ToString());
                await _invoiceRepo.Update(invoice);
            }

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
