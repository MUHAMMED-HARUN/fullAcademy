using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public class GetAllStudentInvoiceQuery : IRequest<TResult<List<StudentInvoiceInfo>>>, IQuery
    {
    }
}
