using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.DTO;
using Gridify;

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

    public sealed class GetAllFeeStructureFilterQueryHandler
        : IRequestHandler<GetAllFeeStructureFilterQuery, TResult<Paging<FeeStructureInfo>>>
    {
        private readonly IFeeStructureRepository _repo;

        public GetAllFeeStructureFilterQueryHandler(IFeeStructureRepository repo) => _repo = repo;

        public async Task<TResult<Paging<FeeStructureInfo>>> Handle(
            GetAllFeeStructureFilterQuery request, CancellationToken cancellationToken) =>
            TResult<Paging<FeeStructureInfo>>.OK(await _repo.GetListFilter(request));
    }
}
