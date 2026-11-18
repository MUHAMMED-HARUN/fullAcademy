using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;

namespace System.Application.Services.Finance.Payment.Query
{
    public sealed class GetPaymentsByInvoiceQuery : IQuery, IRequest<TResult<List<PaymentInfo>>>
    {
        public string StudentInvoiceID { get; set; } = string.Empty;
    }
}
