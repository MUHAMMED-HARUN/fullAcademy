using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.City.Query
{
    public sealed class GetCityByQuery : IRequest<TResult<CityInfo>>, IQuery
    {
        public string Param { get; set; } = string.Empty;
        public enCityParamType ParamType { get; set; } = enCityParamType.ID;
    }
}
