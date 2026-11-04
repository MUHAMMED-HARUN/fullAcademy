using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.Command
{
    public class NewPaymentCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentInvoiceID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
    }
}
