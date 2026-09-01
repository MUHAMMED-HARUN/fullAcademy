using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.Country.Query
{
    class GetAllCountryQueryHandler : IRequestHandler<GetAllCountryQuery, TResult<List<CountryInfo>>>
    {
        ICountryRepository _countryRepository;
        public GetAllCountryQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<TResult<List<CountryInfo>>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<CountryInfo>>.OK(await _countryRepository.GetList());
        }
    }
}
