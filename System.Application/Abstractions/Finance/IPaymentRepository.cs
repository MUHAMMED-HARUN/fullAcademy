using System.Application.Services.Finance.Payment.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;

namespace System.Application.Abstractions.Finance
{
    public interface IPaymentRepository
    {
        Task<string> Add(clsPayment entity);
        Task<bool> Update(clsPayment entity);
        Task<List<PaymentInfo>> GetList();
        Task<PaymentInfo> GetBy(string param, enPaymentParamType paramType);
        Task<clsPayment> GetEntityBy(string param, enPaymentParamType paramType);
        Task<bool> IsExistBy(string param, enPaymentParamType paramType);
    }
}
