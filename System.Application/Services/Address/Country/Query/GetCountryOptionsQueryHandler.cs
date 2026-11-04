using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.Country.Query
{
    public sealed class GetCountryOptionsQueryHandler
        : IRequestHandler<GetCountryOptionsQuery, TResult<List<CountryInfo>>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryOptionsQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<TResult<List<CountryInfo>>> Handle(
            GetCountryOptionsQuery request,
            CancellationToken cancellationToken)
        {
            var countries = await _countryRepository.GetList();
            return TResult<List<CountryInfo>>.OK(countries);
        }
    }
}
