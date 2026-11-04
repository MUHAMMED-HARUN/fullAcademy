using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.DTO;

namespace System.Application.Services.Finance.StudentInvoice.Query
{
    public class GetAllStudentInvoiceQueryHandler : IRequestHandler<GetAllStudentInvoiceQuery, TResult<List<StudentInvoiceInfo>>>
    {
        private readonly IStudentInvoiceRepository _repo;

        public GetAllStudentInvoiceQueryHandler(IStudentInvoiceRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<StudentInvoiceInfo>>> Handle(GetAllStudentInvoiceQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<StudentInvoiceInfo>>.OK(await _repo.GetList());
        }
    }
}
