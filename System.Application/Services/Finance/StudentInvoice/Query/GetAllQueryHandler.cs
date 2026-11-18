using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;
using Gridify;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public class GetAllStudentInvoiceQueryHandler : IRequestHandler<GetAllStudentInvoiceQuery, TResult<Paging<StudentInvoiceInfo>>>
    {
        private readonly IStudentInvoiceRepository _repo;

        public GetAllStudentInvoiceQueryHandler(IStudentInvoiceRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<StudentInvoiceInfo>>> Handle(GetAllStudentInvoiceQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<StudentInvoiceInfo>>.OK(await _repo.GetListFilter(request));
        }
    }
}
