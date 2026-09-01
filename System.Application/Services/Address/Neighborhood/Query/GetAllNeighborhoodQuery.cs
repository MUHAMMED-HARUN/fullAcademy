

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Application.Services.Authentication.Role.DTO;

namespace System.Application.Services.Address.Neighborhood.Query
{
    public class GetAllNeighborhoodQuery : IRequest<TResult<List<NeighborhoodInfo>>>, IQuery
    {
    }
}
