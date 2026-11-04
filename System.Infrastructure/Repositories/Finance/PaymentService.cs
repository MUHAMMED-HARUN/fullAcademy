using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Finance
{
    public class PaymentService : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsPayment> _set;

        public PaymentService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsPayment>();
        }

        public async Task<string> Add(clsPayment entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsPayment entity)
        {
            _set.Update(entity);
            return true;
        }

        public async Task<List<PaymentInfo>> GetList()
        {
            return await _set.Select(x => new PaymentInfo
            {
                PaymentID = x.Id.ToString(),
                StudentInvoiceID = x.StudentInvoiceID.ToString(),
                StudentName = x.StudentInvoice != null && x.StudentInvoice.Student != null && x.StudentInvoice.Student.Person != null
                    ? x.StudentInvoice.Student.Person.FirstName + " " + x.StudentInvoice.Student.Person.LastName
                    : null,
                FeeStructureName = x.StudentInvoice != null && x.StudentInvoice.FeeStructure != null
                    ? x.StudentInvoice.FeeStructure.Name
                    : null,
                Amount = x.Amount,
                PaymentDate = x.PaymentDate,
                PaymentMethod = x.PaymentMethod,
                ReferenceNumber = x.ReferenceNumber,
                Notes = x.Notes
            }).ToListAsync();
        }

        public async Task<PaymentInfo> GetBy(string param, enPaymentParamType paramType)
        {
            return paramType switch
            {
                enPaymentParamType.ID => await _set
                    .Where(x => x.Id == clsUtil.ToGuid(param))
                    .Select(x => new PaymentInfo
                    {
                        PaymentID = x.Id.ToString(),
                        StudentInvoiceID = x.StudentInvoiceID.ToString(),
                        Amount = x.Amount,
                        PaymentDate = x.PaymentDate,
                        PaymentMethod = x.PaymentMethod,
                        ReferenceNumber = x.ReferenceNumber,
                        Notes = x.Notes
                    }).FirstOrDefaultAsync(),

                enPaymentParamType.StudentInvoiceID => await _set
                    .Where(x => x.StudentInvoiceID == clsUtil.ToGuid(param))
                    .Select(x => new PaymentInfo
                    {
                        PaymentID = x.Id.ToString(),
                        StudentInvoiceID = x.StudentInvoiceID.ToString(),
                        Amount = x.Amount,
                        PaymentDate = x.PaymentDate,
                        PaymentMethod = x.PaymentMethod,
                        ReferenceNumber = x.ReferenceNumber,
                        Notes = x.Notes
                    }).FirstOrDefaultAsync(),

                enPaymentParamType.ReferenceNumber => await _set
                    .Where(x => x.ReferenceNumber == param)
                    .Select(x => new PaymentInfo
                    {
                        PaymentID = x.Id.ToString(),
                        StudentInvoiceID = x.StudentInvoiceID.ToString(),
                        Amount = x.Amount,
                        PaymentDate = x.PaymentDate,
                        PaymentMethod = x.PaymentMethod,
                        ReferenceNumber = x.ReferenceNumber,
                        Notes = x.Notes
                    }).FirstOrDefaultAsync(),

                _ => null
            };
        }

        public async Task<clsPayment> GetEntityBy(string param, enPaymentParamType paramType)
        {
            return paramType switch
            {
                enPaymentParamType.ID => await _set.FirstOrDefaultAsync(x => x.Id == clsUtil.ToGuid(param)),
                enPaymentParamType.StudentInvoiceID => await _set.FirstOrDefaultAsync(x => x.StudentInvoiceID == clsUtil.ToGuid(param)),
                enPaymentParamType.ReferenceNumber => await _set.FirstOrDefaultAsync(x => x.ReferenceNumber == param),
                _ => null
            };
        }

        public async Task<bool> IsExistBy(string param, enPaymentParamType paramType)
        {
            return paramType switch
            {
                enPaymentParamType.ID => await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param)),
                enPaymentParamType.StudentInvoiceID => await _set.AnyAsync(x => x.StudentInvoiceID == clsUtil.ToGuid(param)),
                enPaymentParamType.ReferenceNumber => await _set.AnyAsync(x => x.ReferenceNumber == param),
                _ => false
            };
        }
    }
}
