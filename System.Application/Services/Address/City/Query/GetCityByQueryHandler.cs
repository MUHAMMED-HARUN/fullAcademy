using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.City.DTO;

namespace System.Application.Services.Address.City.Query
{
    public sealed class GetCityByQueryHandler : IRequestHandler<GetCityByQuery, TResult<CityInfo>>
    {
        private readonly ICityRepository _cityRepository;

        public GetCityByQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<TResult<CityInfo>> Handle(
            GetCityByQuery request,
            CancellationToken cancellationToken)
        {
            var city = await _cityRepository.GetById(request.Param, request.ParamType);
            return city is null
                ? TResult<CityInfo>.Fail(ApplicationExMsg.CityNotFound)
                : TResult<CityInfo>.OK(city);
        }
    }
}
