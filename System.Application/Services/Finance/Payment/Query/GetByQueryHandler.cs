using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Finance.Payment.DTO;

namespace System.Application.Services.Finance.Payment.Query
{
    public class GetPaymentByQueryHandler : IRequestHandler<GetPaymentByQuery, TResult<PaymentInfo>>
    {
        private readonly IPaymentRepository _repo;

        public GetPaymentByQueryHandler(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<PaymentInfo>> Handle(GetPaymentByQuery request, CancellationToken cancellationToken)
        {
            var info = await _repo.GetBy(request.Param, request.ParamType);
            if (info == null)
                return TResult<PaymentInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<PaymentInfo>.OK(info);
        }
    }
}
