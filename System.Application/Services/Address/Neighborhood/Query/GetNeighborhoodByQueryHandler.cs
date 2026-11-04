using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Neighborhood.DTO;

namespace System.Application.Services.Address.Neighborhood.Query
{
    public sealed class GetNeighborhoodByQueryHandler
        : IRequestHandler<GetNeighborhoodByQuery, TResult<NeighborhoodInfo>>
    {
        private readonly INeighborhoodRepository _neighborhoodRepository;

        public GetNeighborhoodByQueryHandler(INeighborhoodRepository neighborhoodRepository)
        {
            _neighborhoodRepository = neighborhoodRepository;
        }

        public async Task<TResult<NeighborhoodInfo>> Handle(
            GetNeighborhoodByQuery request,
            CancellationToken cancellationToken)
        {
            var neighborhood = await _neighborhoodRepository.GetById(
                request.Param,
                request.ParamType
            );
            return neighborhood is null
                ? TResult<NeighborhoodInfo>.Fail(ApplicationExMsg.NeighborhoodNotFound)
                : TResult<NeighborhoodInfo>.OK(neighborhood);
        }
    }
}
