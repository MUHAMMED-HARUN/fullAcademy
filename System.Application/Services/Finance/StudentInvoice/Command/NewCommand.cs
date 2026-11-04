using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public class NewStudentInvoiceCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentID { get; set; }
        public string FeeStructureID { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? Notes { get; set; }
    }
}

