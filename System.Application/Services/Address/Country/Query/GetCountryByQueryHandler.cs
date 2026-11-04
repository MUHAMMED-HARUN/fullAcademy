using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.Country.Query
{
    public sealed class GetCountryByQueryHandler
        : IRequestHandler<GetCountryByQuery, TResult<CountryInfo>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryByQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<TResult<CountryInfo>> Handle(
            GetCountryByQuery request,
            CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetById(request.Param, request.ParamType);
            return country is null
                ? TResult<CountryInfo>.Fail(ApplicationExMsg.CountryNotFound)
                : TResult<CountryInfo>.OK(country);
        }
    }
}
