using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.Payment.DTO;

namespace System.Application.Services.Finance.Payment.Query
{
    public class GetAllPaymentQueryHandler : IRequestHandler<GetAllPaymentQuery, TResult<List<PaymentInfo>>>
    {
        private readonly IPaymentRepository _repo;

        public GetAllPaymentQueryHandler(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<PaymentInfo>>> Handle(GetAllPaymentQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<PaymentInfo>>.OK(await _repo.GetList());
        }
    }
}
