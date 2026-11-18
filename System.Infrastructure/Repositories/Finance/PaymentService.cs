using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;
using System.Application.Services.Finance.Payment.Query;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Finance
{
    public sealed class PaymentService : IPaymentRepository
    {
        private static readonly IGridifyMapper<PaymentInfo> FilterMapper =
            new GridifyMapper<PaymentInfo>()
                .AddMap(nameof(PaymentInfo.InvoiceLabel), payment => payment.InvoiceLabel)
                .AddMap(nameof(PaymentInfo.StudentName), payment => payment.StudentName)
                .AddMap(nameof(PaymentInfo.AcademicPlanName), payment => payment.AcademicPlanName)
                .AddMap(nameof(PaymentInfo.FeeStructureName), payment => payment.FeeStructureName)
                .AddMap(nameof(PaymentInfo.Amount), payment => payment.Amount)
                .AddMap(nameof(PaymentInfo.PaymentMethod), payment => payment.PaymentMethod)
                .AddMap(nameof(PaymentInfo.PaymentDate), payment => payment.PaymentDate)
                .AddMap(nameof(PaymentInfo.ReferenceNumber), payment => payment.ReferenceNumber)
                .AddMap(nameof(PaymentInfo.IsReversed), payment => payment.IsReversed);

        private readonly AppDbContext _context;
        private readonly DbSet<clsPayment> _set;
        private readonly DbSet<clsPaymentReversal> _reversals;
        private readonly ICurrentUserService _currentUser;

        public PaymentService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsPayment>();
            _reversals = context.Set<clsPaymentReversal>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsPayment entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<string> AddReversal(clsPaymentReversal reversal)
        {
            await _reversals.AddAsync(reversal);
            return reversal.Id.ToString();
        }

        public async Task<Paging<PaymentInfo>> GetListFilter(GetAllPaymentQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);
            return await Project(_set.AsNoTracking().ApplyPaymentReadScope(_currentUser, _context))
                .GridifyAsync(request, FilterMapper);
        }

        public Task<List<PaymentInfo>> GetPaymentsByInvoice(Guid invoiceID) =>
            Project(_set.AsNoTracking()
                .ApplyPaymentReadScope(_currentUser, _context)
                .Where(payment => payment.StudentInvoiceID == invoiceID))
                .OrderByDescending(payment => payment.PaymentDate)
                .ToListAsync();

        public async Task<PaymentInfo?> GetBy(string param, enPaymentParamType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return null;

            var query = _set.AsNoTracking().ApplyPaymentReadScope(_currentUser, _context);
            query = paramType switch
            {
                enPaymentParamType.ID when Guid.TryParse(param, out var paymentID) => query.Where(payment => payment.Id == paymentID),
                enPaymentParamType.StudentInvoiceID when Guid.TryParse(param, out var invoiceID) => query.Where(payment => payment.StudentInvoiceID == invoiceID),
                enPaymentParamType.ReferenceNumber => query.Where(payment => payment.ReferenceNumber == param),
                _ => null,
            };

            return query == null ? null : await Project(query).FirstOrDefaultAsync();
        }

        public async Task<clsPayment?> GetEntityBy(string param, enPaymentParamType paramType)
        {
            if (!Guid.TryParse(param, out var id) || id == Guid.Empty || paramType == enPaymentParamType.ReferenceNumber)
                return null;

            var query = _set.ApplyPaymentReadScope(_currentUser, _context);
            query = paramType switch
            {
                enPaymentParamType.ID => query.Where(payment => payment.Id == id),
                enPaymentParamType.StudentInvoiceID => query.Where(payment => payment.StudentInvoiceID == id),
                _ => null,
            };
            return query == null ? null : await query.FirstOrDefaultAsync();
        }

        public Task<clsPayment?> GetPaymentForReversal(Guid paymentID)
        {
            var lockedPayment = _context.Payments
                .FromSqlInterpolated($"SELECT * FROM [Payments] WITH (UPDLOCK, ROWLOCK) WHERE [Id] = {paymentID}")
                .ApplyPaymentReadScope(_currentUser, _context);
            return lockedPayment.FirstOrDefaultAsync(payment => payment.Id == paymentID);
        }

        public Task<bool> HasReversal(Guid paymentID) => _reversals.AsNoTracking()
            .AnyAsync(reversal => reversal.PaymentID == paymentID);

        private static IQueryable<PaymentInfo> Project(IQueryable<clsPayment> query) =>
            query.Select(payment => new PaymentInfo
            {
                PaymentID = payment.Id.ToString(),
                StudentInvoiceID = payment.StudentInvoiceID.ToString(),
                StudentAcademicEnrollmentID = payment.StudentInvoice.StudentAcademicEnrollmentID.HasValue
                    ? payment.StudentInvoice.StudentAcademicEnrollmentID.Value.ToString()
                    : null,
                StudentName = payment.StudentInvoice.Student.Person.FirstName + " " +
                    payment.StudentInvoice.Student.Person.FatherName + " " +
                    payment.StudentInvoice.Student.Person.LastName,
                AcademicPlanName = payment.StudentInvoice.AcademicPlanNameSnapshot ??
                    (payment.StudentInvoice.StudentAcademicEnrollment != null
                        ? payment.StudentInvoice.StudentAcademicEnrollment.AcademicPlan.Name
                        : payment.StudentInvoice.FeeStructure.AcademicPlan.Name),
                FeeStructureName = payment.StudentInvoice.FeeStructureNameSnapshot ?? payment.StudentInvoice.FeeStructure.Name,
                InvoiceLabel = payment.StudentInvoice.Student.Person.FirstName + " " +
                    payment.StudentInvoice.Student.Person.LastName + " — " +
                    (payment.StudentInvoice.AcademicPlanNameSnapshot ?? payment.StudentInvoice.FeeStructure.AcademicPlan.Name) + " / " +
                    (payment.StudentInvoice.FeeStructureNameSnapshot ?? payment.StudentInvoice.FeeStructure.Name),
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod,
                ReferenceNumber = payment.ReferenceNumber,
                Notes = payment.Notes,
                IsReversed = payment.Reversal != null,
                ReversalReason = payment.Reversal != null ? payment.Reversal.Reason : null,
                ReversedAt = payment.Reversal != null ? payment.Reversal.ReversedAt : null,
                ReversedBy = payment.Reversal != null ? payment.Reversal.ReversedBy : null,
            });
    }
}
