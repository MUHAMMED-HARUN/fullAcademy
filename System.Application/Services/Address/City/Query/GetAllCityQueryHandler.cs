using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.City.Query
{
    class GetAllCityQueryHandler : IRequestHandler<GetAllCityQuery, TResult<List<CityInfo>>>
    {
        ICityRepository _cityRepo;
        public GetAllCityQueryHandler( ICityRepository cityRepository)
        {
            _cityRepo = cityRepository;
        }
        public async Task<TResult<List<CityInfo>>> Handle(GetAllCityQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<CityInfo>>.OK(await _cityRepo.GetList());
        }
    }
}
