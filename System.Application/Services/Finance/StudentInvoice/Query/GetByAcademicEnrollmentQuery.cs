using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public sealed class GetByAcademicEnrollmentQuery : IQuery, IRequest<TResult<List<StudentInvoiceInfo>>>
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
    }
}
