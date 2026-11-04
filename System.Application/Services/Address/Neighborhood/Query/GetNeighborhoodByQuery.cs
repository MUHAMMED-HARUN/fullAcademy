using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Neighborhood.Query
{
    public sealed class GetNeighborhoodByQuery : IRequest<TResult<NeighborhoodInfo>>, IQuery
    {
        public string Param { get; set; } = string.Empty;
        public enNeighborhoodParamType ParamType { get; set; } = enNeighborhoodParamType.ID;
    }
}
