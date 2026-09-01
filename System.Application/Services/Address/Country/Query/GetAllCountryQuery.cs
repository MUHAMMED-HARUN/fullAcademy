

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Authentication.Role.DTO;

namespace System.Application.Services.Address.Country.Query
{
    public class GetAllCountryQuery : IRequest<TResult<List<CountryInfo>>>, IQuery
    {
    }
}
