using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.DTO;

namespace System.Application.Services.Finance.FeeStructure.Query
{
    public class GetAllFeeStructureQueryHandler : IRequestHandler<GetAllFeeStructureQuery, TResult<List<FeeStructureInfo>>>
    {
        private readonly IFeeStructureRepository _repo;

        public GetAllFeeStructureQueryHandler(IFeeStructureRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<FeeStructureInfo>>> Handle(GetAllFeeStructureQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<FeeStructureInfo>>.OK(await _repo.GetList());
        }
    }
}
