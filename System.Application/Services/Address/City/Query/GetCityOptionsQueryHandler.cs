using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;

namespace System.Application.Services.Address.City.Query
{
    public sealed class GetCityOptionsQueryHandler
        : IRequestHandler<GetCityOptionsQuery, TResult<List<CityInfo>>>
    {
        private readonly ICityRepository _cityRepository;

        public GetCityOptionsQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<TResult<List<CityInfo>>> Handle(
            GetCityOptionsQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<List<CityInfo>>.OK(await _cityRepository.GetList());
        }
    }
}
