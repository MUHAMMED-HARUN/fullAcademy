using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public sealed class GetStudentFinanceSummaryQuery : IQuery, IRequest<TResult<StudentFinanceSummaryInfo>>
    {
        public string? StudentID { get; set; }
    }
}
