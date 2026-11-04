using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.Query
{
    public class GetPaymentByQuery : IRequest<TResult<PaymentInfo>>, IQuery
    {
        public string Param { get; set; }
        public enPaymentParamType ParamType { get; set; }
    }
}
