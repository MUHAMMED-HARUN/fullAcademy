using System.Application.Services.Finance.StudentInvoice.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;

namespace System.Application.Abstractions.Finance
{
    public interface IStudentInvoiceRepository
    {
        Task<string> Add(clsStudentInvoice entity);
        Task<bool> Update(clsStudentInvoice entity);
        Task<List<StudentInvoiceInfo>> GetList();
        Task<StudentInvoiceInfo> GetBy(string param, enStudentInvoiceParamType paramType);
        Task<clsStudentInvoice> GetEntityBy(string param, enStudentInvoiceParamType paramType);
        Task<bool> IsExistBy(string param, enStudentInvoiceParamType paramType);
    }
}
