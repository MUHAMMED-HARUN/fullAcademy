using Gridify;
using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.Country.Query
{
    public class GetAllCountryQueryHandler : IRequestHandler<GetAllCountryQuery, TResult<Paging<CountryInfo>>>
    {
        ICountryRepository _countryRepository;
        public GetAllCountryQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<TResult<Paging<CountryInfo>>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<CountryInfo>>.OK(await _countryRepository.GetListFilter(request));
        }
    }
}
