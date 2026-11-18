using System.Application.Common.Models;
using System.Application.Services.Finance.Payment.DTO;

namespace System.Application.Services.Finance.Payment.Query
{
    public sealed class GetAllPaymentQuery : BaseFilterQuery<PaymentInfo>
    {
        public GetAllPaymentQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
