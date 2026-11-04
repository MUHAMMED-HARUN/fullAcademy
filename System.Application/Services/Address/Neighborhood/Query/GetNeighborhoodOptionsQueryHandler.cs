using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Neighborhood.DTO;

namespace System.Application.Services.Address.Neighborhood.Query
{
    public sealed class GetNeighborhoodOptionsQueryHandler
        : IRequestHandler<GetNeighborhoodOptionsQuery, TResult<List<NeighborhoodInfo>>>
    {
        private readonly INeighborhoodRepository _neighborhoodRepository;

        public GetNeighborhoodOptionsQueryHandler(INeighborhoodRepository neighborhoodRepository)
        {
            _neighborhoodRepository = neighborhoodRepository;
        }

        public async Task<TResult<List<NeighborhoodInfo>>> Handle(
            GetNeighborhoodOptionsQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<List<NeighborhoodInfo>>.OK(await _neighborhoodRepository.GetList());
        }
    }
}
