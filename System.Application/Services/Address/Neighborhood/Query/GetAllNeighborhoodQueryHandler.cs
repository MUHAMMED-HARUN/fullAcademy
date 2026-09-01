using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;
using System.Application.Services.Address.District.Query;
using System.Application.Services.Address.Neighborhood.DTO;

namespace System.Application.Services.Address.Neighborhood.Query
{
    class GetAllNeighborhoodQueryHandler : IRequestHandler<GetAllNeighborhoodQuery, TResult<List<NeighborhoodInfo>>>
    {
        INeighborhoodRepository _neighborhoodRepository;
        public GetAllNeighborhoodQueryHandler(INeighborhoodRepository neighborhood)
        {
            _neighborhoodRepository = neighborhood;
        }
        public async Task<TResult<List<NeighborhoodInfo>>> Handle(GetAllNeighborhoodQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<NeighborhoodInfo>>.OK(await _neighborhoodRepository.GetList());
        }
    }
}
