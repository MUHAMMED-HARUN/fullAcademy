

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;
using System.Application.Services.Authentication.Role.DTO;

namespace System.Application.Services.Address.District.Query
{
    public class GetAllDistrictQuery : IRequest<TResult<List<DistrictInfo>>>, IQuery
    {
    }
}
