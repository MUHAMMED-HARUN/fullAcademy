using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Finance.FeeStructure.DTO;

namespace System.Application.Services.Finance.FeeStructure.Query
{
    public class GetFeeStructureByQueryHandler : IRequestHandler<GetFeeStructureByQuery, TResult<FeeStructureInfo>>
    {
        private readonly IFeeStructureRepository _repo;

        public GetFeeStructureByQueryHandler(IFeeStructureRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<FeeStructureInfo>> Handle(GetFeeStructureByQuery request, CancellationToken cancellationToken)
        {
            var info = await _repo.GetBy(request.Param, request.ParamType);
            if (info == null)
                return TResult<FeeStructureInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<FeeStructureInfo>.OK(info);
        }
    }
}
