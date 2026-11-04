using Gridify;
using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Neighborhood.DTO;

namespace System.Application.Services.Address.Neighborhood.Query
{
    public class GetAllNeighborhoodQueryHandler : IRequestHandler<GetAllNeighborhoodQuery, TResult<Paging<NeighborhoodInfo>>>
    {
        INeighborhoodRepository _neighborhoodRepository;
        public GetAllNeighborhoodQueryHandler(INeighborhoodRepository neighborhood)
        {
            _neighborhoodRepository = neighborhood;
        }
        public async Task<TResult<Paging<NeighborhoodInfo>>> Handle(GetAllNeighborhoodQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<NeighborhoodInfo>>.OK(await _neighborhoodRepository.GetListFilter(request));
        }
    }
}
