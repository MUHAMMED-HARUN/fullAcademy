using Gridify;
using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.City.Query
{
    public class GetAllCityQueryHandler : IRequestHandler<GetAllCityQuery, TResult<Paging<CityInfo>>>
    {
        ICityRepository _cityRepo;
        public GetAllCityQueryHandler( ICityRepository cityRepository)
        {
            _cityRepo = cityRepository;
        }
        public async Task<TResult<Paging<CityInfo>>> Handle(GetAllCityQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<CityInfo>>.OK(await _cityRepo.GetListFilter(request));
        }
    }
}
