using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Country.Query
{
    public sealed class GetCountryByQuery : IRequest<TResult<CountryInfo>>, IQuery
    {
        public string Param { get; set; } = string.Empty;
        public enCountryParamType ParamType { get; set; } = enCountryParamType.ID;
    }
}
