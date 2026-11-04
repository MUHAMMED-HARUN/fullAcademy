using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Finance.Payment.Command
{
    public class DeletePaymentCommand : ICommand, IRequest<TResult<string>>
    {
        public string PaymentID { get; set; }
    }
}
