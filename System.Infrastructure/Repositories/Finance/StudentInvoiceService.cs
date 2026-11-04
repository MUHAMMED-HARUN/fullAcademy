using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Finance
{
    public class StudentInvoiceService : IStudentInvoiceRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsStudentInvoice> _set;

        public StudentInvoiceService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsStudentInvoice>();
        }

        public async Task<string> Add(clsStudentInvoice entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsStudentInvoice entity)
        {
            _set.Update(entity);
            return true;
        }

        public async Task<List<StudentInvoiceInfo>> GetList()
        {
            return await _set.Select(x => new StudentInvoiceInfo
            {
                StudentInvoiceID = x.Id.ToString(),
                StudentID = x.StudentID.ToString(),
                StudentName = x.Student != null ? x.Student.Person.FirstName + " " + x.Student.Person.LastName : null,
                FeeStructureID = x.FeeStructureID.ToString(),
                FeeStructureName = x.FeeStructure != null ? x.FeeStructure.Name : null,
                IssueDate = x.IssueDate,
                DueDate = x.DueDate,
                OriginalAmount = x.OriginalAmount,
                DiscountAmount = x.DiscountAmount,
                PaidAmount = x.OriginalAmount - x.DiscountAmount - x.RemainingAmount,
                RemainingAmount = x.RemainingAmount,
                Status = x.Status,
                Notes = x.Notes
            }).ToListAsync();
        }

        public async Task<StudentInvoiceInfo> GetBy(string param, enStudentInvoiceParamType paramType)
        {
            return paramType switch
            {
                enStudentInvoiceParamType.ID => await _set
                    .Where(x => x.Id == clsUtil.ToGuid(param))
                    .Select(x => new StudentInvoiceInfo
                    {
                        StudentInvoiceID = x.Id.ToString(),
                        StudentID = x.StudentID.ToString(),
                        StudentName = x.Student != null ? x.Student.Person.FirstName + " " + x.Student.Person.LastName : null,
                        FeeStructureID = x.FeeStructureID.ToString(),
                        FeeStructureName = x.FeeStructure != null ? x.FeeStructure.Name : null,
                        IssueDate = x.IssueDate,
                        DueDate = x.DueDate,
                        OriginalAmount = x.OriginalAmount,
                        DiscountAmount = x.DiscountAmount,
                        RemainingAmount = x.RemainingAmount,
                        Status = x.Status,
                        Notes = x.Notes
                    }).FirstOrDefaultAsync(),

                enStudentInvoiceParamType.StudentID => await _set
                    .Where(x => x.StudentID == clsUtil.ToGuid(param))
                    .Select(x => new StudentInvoiceInfo
                    {
                        StudentInvoiceID = x.Id.ToString(),
                        StudentID = x.StudentID.ToString(),
                        StudentName = x.Student != null ? x.Student.Person.FirstName + " " + x.Student.Person.LastName : null,
                        FeeStructureID = x.FeeStructureID.ToString(),
                        FeeStructureName = x.FeeStructure != null ? x.FeeStructure.Name : null,
                        IssueDate = x.IssueDate,
                        DueDate = x.DueDate,
                        OriginalAmount = x.OriginalAmount,
                        DiscountAmount = x.DiscountAmount,
                        RemainingAmount = x.RemainingAmount,
                        Status = x.Status,
                        Notes = x.Notes
                    }).FirstOrDefaultAsync(),

                _ => null
            };
        }

        public async Task<clsStudentInvoice> GetEntityBy(string param, enStudentInvoiceParamType paramType)
        {
            return paramType switch
            {
                enStudentInvoiceParamType.ID => await _set.FirstOrDefaultAsync(x => x.Id == clsUtil.ToGuid(param)),
                enStudentInvoiceParamType.StudentID => await _set.FirstOrDefaultAsync(x => x.StudentID == clsUtil.ToGuid(param)),
                enStudentInvoiceParamType.FeeStructureID => await _set.FirstOrDefaultAsync(x => x.FeeStructureID == clsUtil.ToGuid(param)),
                _ => null
            };
        }

        public async Task<bool> IsExistBy(string param, enStudentInvoiceParamType paramType)
        {
            return paramType switch
            {
                enStudentInvoiceParamType.ID => await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param)),
                enStudentInvoiceParamType.StudentID => await _set.AnyAsync(x => x.StudentID == clsUtil.ToGuid(param)),
                enStudentInvoiceParamType.FeeStructureID => await _set.AnyAsync(x => x.FeeStructureID == clsUtil.ToGuid(param)),
                _ => false
            };
        }
    }
}
