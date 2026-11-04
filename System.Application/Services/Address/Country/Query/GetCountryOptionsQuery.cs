using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.Country.Query
{
    public sealed class GetCountryOptionsQuery : IRequest<TResult<List<CountryInfo>>>, IQuery
    {
    }
}
