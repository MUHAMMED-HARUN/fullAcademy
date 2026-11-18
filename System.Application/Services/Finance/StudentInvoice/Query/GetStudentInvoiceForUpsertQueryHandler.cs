using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Finance.StudentInvoice.DTO;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public sealed class GetStudentInvoiceForUpsertQueryHandler
        : IRequestHandler<GetStudentInvoiceForUpsertQuery, TResult<StudentInvoiceInfo>>
    {
        private readonly IStudentInvoiceRepository _repository;
        public GetStudentInvoiceForUpsertQueryHandler(IStudentInvoiceRepository repository) => _repository = repository;

        public async Task<TResult<StudentInvoiceInfo>> Handle(
            GetStudentInvoiceForUpsertQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentInvoiceID, out var invoiceID) || invoiceID == Guid.Empty)
                return TResult<StudentInvoiceInfo>.Fail("StudentInvoiceID must be a valid identifier.");
            var invoice = await _repository.GetBy(invoiceID.ToString(), enStudentInvoiceParamType.ID);
            return invoice.Count == 0
                ? TResult<StudentInvoiceInfo>.Fail(ApplicationExMsg.NotFound)
                : TResult<StudentInvoiceInfo>.OK(invoice[0]);
        }
    }
}
