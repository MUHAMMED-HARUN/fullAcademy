using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Address.DTO;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Address.Query
{
    public class GetAddressByQuery : IRequest<TResult<AddressInfo>>, IQuery
    {
        public string Param { get; set; } = string.Empty;
        public enAddressParamType ParamType { get; set; } = enAddressParamType.ID;
    }
}
