using System.Application.Services.Finance.StudentInvoice.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Application.Services.Finance.StudentInvoice.Query;
using System.Application.Common;
using Gridify;

namespace System.Application.Abstractions.Finance
{
    public interface IStudentInvoiceRepository
    {
        Task<string> Add(clsStudentInvoice entity);
        Task<bool> Update(clsStudentInvoice entity);
        Task<Paging<StudentInvoiceInfo>> GetListFilter(GetAllStudentInvoiceQuery request);
        Task<List<StudentInvoiceInfo>> GetBy(string param, enStudentInvoiceParamType paramType);
        Task<List<StudentInvoiceInfo>> GetByAcademicEnrollment(Guid enrollmentID);
        Task<StudentFinanceSummaryInfo?> GetStudentFinanceSummary(Guid studentID);
        Task<clsStudentInvoice?> GetEntityBy(string param, enStudentInvoiceParamType paramType);
        Task<clsStudentInvoice?> GetEntityForPayment(Guid invoiceID);
        Task<bool> IsExistBy(string param, enStudentInvoiceParamType paramType);
        Task<bool> HasPayments(Guid invoiceID);
        Task<clsStudentInvoice?> GetGeneratedInvoice(Guid enrollmentID, Guid feeStructureID, DateOnly periodStart, DateOnly periodEnd);
    }
}
