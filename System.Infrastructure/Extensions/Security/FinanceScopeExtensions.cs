using System.Application.Abstractions.Auth;
using System.Domain.Entities.Finance;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class FinanceScopeExtensions
    {
        public static IQueryable<clsFeeStructure> ApplyFeeStructureReadScope(
            this IQueryable<clsFeeStructure> query,
            ICurrentUserService currentUser)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            // Fee catalog and pricing management is staff-only. Students see their invoice snapshot.
            if (currentUser.IsAdmin)
                return query;

            return query.Where(_ => false);
        }

        public static IQueryable<clsStudentInvoice> ApplyStudentInvoiceReadScope(
            this IQueryable<clsStudentInvoice> query,
            ICurrentUserService currentUser)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            if (currentUser.IsAdmin)
                return query;

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(invoice => invoice.StudentID == studentID);
            }

            return query.Where(_ => false);
        }

        public static IQueryable<clsPayment> ApplyPaymentReadScope(
            this IQueryable<clsPayment> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            if (currentUser.IsAdmin)
                return query;

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(payment => context.StudentInvoices.Any(invoice =>
                    invoice.Id == payment.StudentInvoiceID && invoice.StudentID == studentID));
            }

            return query.Where(_ => false);
        }
    }
}
