using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Common.Models;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public sealed class GetAllStudentInvoiceQuery : BaseFilterQuery<StudentInvoiceInfo>
    {
        public bool PayableOnly { get; set; }

        public GetAllStudentInvoiceQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
