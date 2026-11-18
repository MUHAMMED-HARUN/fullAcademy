using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;

namespace System.Application.Services.Finance.Payment.Query
{
    public sealed class GetPaymentsByInvoiceQueryHandler
        : IRequestHandler<GetPaymentsByInvoiceQuery, TResult<List<PaymentInfo>>>
    {
        private readonly IPaymentRepository _repository;
        public GetPaymentsByInvoiceQueryHandler(IPaymentRepository repository) => _repository = repository;

        public async Task<TResult<List<PaymentInfo>>> Handle(
            GetPaymentsByInvoiceQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentInvoiceID, out var invoiceID) || invoiceID == Guid.Empty)
                return TResult<List<PaymentInfo>>.Fail("StudentInvoiceID must be a valid identifier.");
            return TResult<List<PaymentInfo>>.OK(await _repository.GetPaymentsByInvoice(invoiceID));
        }
    }
}
