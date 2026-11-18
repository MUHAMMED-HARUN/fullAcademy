using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;
using Gridify;

namespace System.Application.Services.Finance.Payment.Query
{
    public class GetAllPaymentQueryHandler : IRequestHandler<GetAllPaymentQuery, TResult<Paging<PaymentInfo>>>
    {
        private readonly IPaymentRepository _repo;

        public GetAllPaymentQueryHandler(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<PaymentInfo>>> Handle(GetAllPaymentQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<PaymentInfo>>.OK(await _repo.GetListFilter(request));
        }
    }
}
