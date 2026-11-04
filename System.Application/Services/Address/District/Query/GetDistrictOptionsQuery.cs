using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.District.DTO;

namespace System.Application.Services.Address.District.Query
{
    public sealed class GetDistrictOptionsQuery : IRequest<TResult<List<DistrictInfo>>>, IQuery
    {
    }
}
