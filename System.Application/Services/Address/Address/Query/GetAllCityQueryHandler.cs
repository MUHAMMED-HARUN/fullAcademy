using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Address.DTO;
using System.Application.Services.Address.Address.Query;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.City.Query
{
    class GetAddressByQueryHandler : IRequestHandler<GetAddressByQuery, TResult<AddressInfo>>
    {
        IAddressRepository _repo;
        public GetAddressByQueryHandler( IAddressRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<AddressInfo>> Handle(GetAddressByQuery request, CancellationToken cancellationToken)
        {
            AddressInfo data = await _repo.GetBy(request.Param,(enAddressParamType)request.ParamType);
            return  TResult<AddressInfo>.OK(data);
        }
    }
}
