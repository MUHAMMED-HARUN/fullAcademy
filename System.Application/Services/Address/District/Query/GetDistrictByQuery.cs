using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.District.DTO;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.District.Query
{
    public sealed class GetDistrictByQuery : IRequest<TResult<DistrictInfo>>, IQuery
    {
        public string Param { get; set; } = string.Empty;
        public enDistrictParamType ParamType { get; set; } = enDistrictParamType.ID;
    }
}
