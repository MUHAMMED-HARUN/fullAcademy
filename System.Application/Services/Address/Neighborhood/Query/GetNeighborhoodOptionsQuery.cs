using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Neighborhood.DTO;

namespace System.Application.Services.Address.Neighborhood.Query
{
    public sealed class GetNeighborhoodOptionsQuery
        : IRequest<TResult<List<NeighborhoodInfo>>>, IQuery
    {
    }
}
