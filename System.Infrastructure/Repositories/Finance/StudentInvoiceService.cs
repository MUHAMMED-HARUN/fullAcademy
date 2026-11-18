using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;
using System.Application.Services.Finance.StudentInvoice.Query;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Finance
{
    public sealed class StudentInvoiceService : IStudentInvoiceRepository
    {
        private static readonly IGridifyMapper<StudentInvoiceInfo> FilterMapper =
            new GridifyMapper<StudentInvoiceInfo>()
                .AddMap(nameof(StudentInvoiceInfo.StudentName), invoice => invoice.StudentName)
                .AddMap(nameof(StudentInvoiceInfo.FeeStructureName), invoice => invoice.FeeStructureName)
                .AddMap(nameof(StudentInvoiceInfo.AcademicPlanName), invoice => invoice.AcademicPlanName)
                .AddMap(nameof(StudentInvoiceInfo.Status), invoice => invoice.Status)
                .AddMap(nameof(StudentInvoiceInfo.IssueDate), invoice => invoice.IssueDate)
                .AddMap(nameof(StudentInvoiceInfo.DueDate), invoice => invoice.DueDate)
                .AddMap(nameof(StudentInvoiceInfo.BillingPeriodStart), invoice => invoice.BillingPeriodStart)
                .AddMap(nameof(StudentInvoiceInfo.BillingPeriodEnd), invoice => invoice.BillingPeriodEnd)
                .AddMap(nameof(StudentInvoiceInfo.RemainingAmount), invoice => invoice.RemainingAmount);

        private readonly AppDbContext _context;
        private readonly DbSet<clsStudentInvoice> _set;
        private readonly ICurrentUserService _currentUser;

        public StudentInvoiceService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsStudentInvoice>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsStudentInvoice entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public Task<bool> Update(clsStudentInvoice entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        public async Task<Paging<StudentInvoiceInfo>> GetListFilter(GetAllStudentInvoiceQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking().ApplyStudentInvoiceReadScope(_currentUser);
            if (request.PayableOnly)
            {
                query = query.Where(invoice =>
                    invoice.RemainingAmount > 0 &&
                    invoice.Status != enInvoiceStatus.Cancelled &&
                    invoice.Status != enInvoiceStatus.Paid);
            }

            var projected = Project(query);
            return await projected.GridifyAsync(request, FilterMapper);
        }

        public async Task<List<StudentInvoiceInfo>> GetBy(string param, enStudentInvoiceParamType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return new List<StudentInvoiceInfo>();

            var query = _set.AsNoTracking().ApplyStudentInvoiceReadScope(_currentUser);
            query = paramType switch
            {
                enStudentInvoiceParamType.ID when Guid.TryParse(param, out var invoiceID) => query.Where(x => x.Id == invoiceID),
                enStudentInvoiceParamType.StudentID when Guid.TryParse(param, out var studentID) => query.Where(x => x.StudentID == studentID),
                enStudentInvoiceParamType.FeeStructureID when Guid.TryParse(param, out var feeStructureID) => query.Where(x => x.FeeStructureID == feeStructureID),
                enStudentInvoiceParamType.Status when int.TryParse(param, out var status) => query.Where(x => (int)x.Status == status),
                _ => null,
            };

            return query == null ? new List<StudentInvoiceInfo>() : await Project(query).ToListAsync();
        }

        public Task<List<StudentInvoiceInfo>> GetByAcademicEnrollment(Guid enrollmentID) =>
            Project(_set.AsNoTracking()
                .ApplyStudentInvoiceReadScope(_currentUser)
                .Where(invoice => invoice.StudentAcademicEnrollmentID == enrollmentID))
                .OrderByDescending(invoice => invoice.IssueDate)
                .ToListAsync();

        public async Task<StudentFinanceSummaryInfo?> GetStudentFinanceSummary(Guid studentID)
        {
            var student = await _context.Students.AsNoTracking()
                .ApplyStudentReadScope(_currentUser, _context)
                .Where(item => item.Id == studentID)
                .Select(item => new
                {
                    item.Id,
                    Name = item.Person.FirstName + " " + item.Person.FatherName + " " + item.Person.LastName,
                })
                .FirstOrDefaultAsync();
            if (student == null)
                return null;

            var today = DateOnly.FromDateTime(DateTime.Today);
            var invoices = _set.AsNoTracking()
                .ApplyStudentInvoiceReadScope(_currentUser)
                .Where(invoice => invoice.StudentID == studentID && invoice.Status != enInvoiceStatus.Cancelled);

            return new StudentFinanceSummaryInfo
            {
                StudentID = student.Id.ToString(),
                StudentName = student.Name,
                InvoiceCount = await invoices.CountAsync(),
                TotalOriginalAmount = await invoices.SumAsync(invoice => (decimal?)invoice.OriginalAmount) ?? 0,
                TotalDiscountAmount = await invoices.SumAsync(invoice => (decimal?)invoice.DiscountAmount) ?? 0,
                TotalPaidAmount = await invoices.SumAsync(invoice => (decimal?)(invoice.OriginalAmount - invoice.DiscountAmount - invoice.RemainingAmount)) ?? 0,
                TotalRemainingAmount = await invoices.SumAsync(invoice => (decimal?)invoice.RemainingAmount) ?? 0,
                OverdueAmount = await invoices
                    .Where(invoice => invoice.DueDate.HasValue && invoice.DueDate.Value < today && invoice.Status != enInvoiceStatus.Paid && invoice.Status != enInvoiceStatus.Cancelled)
                    .SumAsync(invoice => (decimal?)invoice.RemainingAmount) ?? 0,
            };
        }

        public async Task<clsStudentInvoice?> GetEntityBy(string param, enStudentInvoiceParamType paramType)
        {
            if (!Guid.TryParse(param, out var id) || id == Guid.Empty)
                return null;

            var query = _set.ApplyStudentInvoiceReadScope(_currentUser);
            query = paramType switch
            {
                enStudentInvoiceParamType.ID => query.Where(x => x.Id == id),
                enStudentInvoiceParamType.StudentID => query.Where(x => x.StudentID == id),
                enStudentInvoiceParamType.FeeStructureID => query.Where(x => x.FeeStructureID == id),
                _ => null,
            };
            return query == null ? null : await query.FirstOrDefaultAsync();
        }

        public Task<clsStudentInvoice?> GetEntityForPayment(Guid invoiceID)
        {
            var lockedInvoice = _context.StudentInvoices
                .FromSqlInterpolated($"SELECT * FROM [StudentInvoices] WITH (UPDLOCK, ROWLOCK) WHERE [Id] = {invoiceID}")
                .ApplyStudentInvoiceReadScope(_currentUser);
            return lockedInvoice.FirstOrDefaultAsync(invoice => invoice.Id == invoiceID);
        }

        public async Task<bool> IsExistBy(string param, enStudentInvoiceParamType paramType)
        {
            if (!Guid.TryParse(param, out var id) || id == Guid.Empty)
                return false;
            var query = _set.AsNoTracking().ApplyStudentInvoiceReadScope(_currentUser);
            query = paramType switch
            {
                enStudentInvoiceParamType.ID => query.Where(x => x.Id == id),
                enStudentInvoiceParamType.StudentID => query.Where(x => x.StudentID == id),
                enStudentInvoiceParamType.FeeStructureID => query.Where(x => x.FeeStructureID == id),
                _ => null,
            };
            return query != null && await query.AnyAsync();
        }

        public Task<bool> HasPayments(Guid invoiceID) => _context.Payments.AsNoTracking()
            .AnyAsync(payment => payment.StudentInvoiceID == invoiceID);

        public Task<clsStudentInvoice?> GetGeneratedInvoice(
            Guid enrollmentID, Guid feeStructureID, DateOnly periodStart, DateOnly periodEnd) =>
            _set.ApplyStudentInvoiceReadScope(_currentUser).FirstOrDefaultAsync(invoice =>
                invoice.StudentAcademicEnrollmentID == enrollmentID &&
                invoice.FeeStructureID == feeStructureID &&
                invoice.BillingPeriodStart == periodStart &&
                invoice.BillingPeriodEnd == periodEnd);

        private static IQueryable<StudentInvoiceInfo> Project(IQueryable<clsStudentInvoice> query) =>
            query.Select(invoice => new StudentInvoiceInfo
            {
                StudentInvoiceID = invoice.Id.ToString(),
                StudentID = invoice.StudentID.ToString(),
                StudentName = invoice.Student.Person.FirstName + " " + invoice.Student.Person.FatherName + " " + invoice.Student.Person.LastName,
                StudentAcademicEnrollmentID = invoice.StudentAcademicEnrollmentID.HasValue ? invoice.StudentAcademicEnrollmentID.Value.ToString() : null,
                AcademicPlanID = invoice.StudentAcademicEnrollment != null
                    ? invoice.StudentAcademicEnrollment.AcademicPlanID.ToString()
                    : invoice.FeeStructure.AcademicPlanID.ToString(),
                AcademicPlanName = invoice.AcademicPlanNameSnapshot ??
                    (invoice.StudentAcademicEnrollment != null ? invoice.StudentAcademicEnrollment.AcademicPlan.Name : invoice.FeeStructure.AcademicPlan.Name),
                AcademicPlanVersion = invoice.AcademicPlanVersionSnapshot ??
                    (invoice.StudentAcademicEnrollment != null ? invoice.StudentAcademicEnrollment.AcademicPlanVersion : invoice.FeeStructure.AcademicPlan.Version),
                FeeStructureID = invoice.FeeStructureID.ToString(),
                FeeStructureName = invoice.FeeStructureNameSnapshot ?? invoice.FeeStructure.Name,
                BillingPeriodStart = invoice.BillingPeriodStart,
                BillingPeriodEnd = invoice.BillingPeriodEnd,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                OriginalAmount = invoice.OriginalAmount,
                DiscountAmount = invoice.DiscountAmount,
                PaidAmount = invoice.OriginalAmount - invoice.DiscountAmount - invoice.RemainingAmount,
                RemainingAmount = invoice.RemainingAmount,
                Status = invoice.Status,
                Notes = invoice.Notes,
                HasPayments = invoice.Payments.Any(),
            });
    }
}
