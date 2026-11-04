using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;

namespace System.Application.Services.Finance.Payment.Query
{
    public class GetAllPaymentQuery : IRequest<TResult<List<PaymentInfo>>>, IQuery
    {
    }
}
