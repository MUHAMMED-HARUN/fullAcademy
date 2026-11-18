using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public class GetStudentInvoiceByQueryHandler : IRequestHandler<GetStudentInvoiceByQuery, TResult<List<StudentInvoiceInfo>>>
    {
        private readonly IStudentInvoiceRepository _repo;

        public GetStudentInvoiceByQueryHandler(IStudentInvoiceRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<StudentInvoiceInfo>>> Handle(GetStudentInvoiceByQuery request, CancellationToken cancellationToken)
        {
            var info = await _repo.GetBy(request.Param, request.ParamType);
            if (info.Count == 0)
                return TResult<List<StudentInvoiceInfo>>.Fail(ApplicationExMsg.NotFound);

            return TResult<List<StudentInvoiceInfo>>.OK(info);
        }
    }
}
