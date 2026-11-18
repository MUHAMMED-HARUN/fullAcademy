using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Finance.Payment.Command
{
    public sealed class ReversePaymentCommand : ITransaction, IRequest<TResult<string>>
    {
        public string PaymentID { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
