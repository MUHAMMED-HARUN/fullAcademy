using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public class GetStudentInvoiceByQuery : IRequest<TResult<StudentInvoiceInfo>>, IQuery
    {
        public string Param { get; set; }
        public enStudentInvoiceParamType ParamType { get; set; }
    }
}
