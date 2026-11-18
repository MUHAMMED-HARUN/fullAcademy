using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public sealed class GenerateInvoicesForEnrollmentCommand : ITransaction, IRequest<TResult<StudentInvoiceInfo>>
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string FeeStructureID { get; set; } = string.Empty;
        public DateOnly BillingPeriodStart { get; set; }
        public DateOnly BillingPeriodEnd { get; set; }
        public string? Notes { get; set; }
    }
}
