using System.Application.Services.Finance.Payment.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Application.Services.Finance.Payment.Query;
using System.Application.Common;
using Gridify;

namespace System.Application.Abstractions.Finance
{
    public interface IPaymentRepository
    {
        Task<string> Add(clsPayment entity);
        Task<string> AddReversal(clsPaymentReversal reversal);
        Task<Paging<PaymentInfo>> GetListFilter(GetAllPaymentQuery request);
        Task<List<PaymentInfo>> GetPaymentsByInvoice(Guid invoiceID);
        Task<PaymentInfo?> GetBy(string param, enPaymentParamType paramType);
        Task<clsPayment?> GetEntityBy(string param, enPaymentParamType paramType);
        Task<clsPayment?> GetPaymentForReversal(Guid paymentID);
        Task<bool> HasReversal(Guid paymentID);
    }
}
