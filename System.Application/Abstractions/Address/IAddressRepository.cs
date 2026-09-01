

using System.Application.Services.Address.Address.DTO;
using System.Domain.Entities.Address;
using System.Domain.Enums.Address;

namespace System.Application.Abstractions.Repository
{
    public interface IAddressRepository {
        Task<string> Add(clsAddress address);// return CountryID as String
        Task<bool> Update(clsAddress address);
        Task<AddressInfo> GetBy(string param, enAddressParamType paramType);
        Task<clsAddress> GetEntityBy(string param, enAddressParamType paramType);
    }
}
