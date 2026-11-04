using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public class DeleteStudentInvoiceCommand : ICommand, IRequest<TResult<string>>
    {
        public string StudentInvoiceID { get; set; }
    }
}
