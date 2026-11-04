using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public class UpdateStudentInvoiceCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentInvoiceID { get; set; }
        public DateOnly DueDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public enInvoiceStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
