using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;

namespace System.Application.Services.Address.City.Query
{
    public sealed class GetCityOptionsQuery : IRequest<TResult<List<CityInfo>>>, IQuery
    {
    }
}
