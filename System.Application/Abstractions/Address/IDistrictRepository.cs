using System.Application.Services.Address.District.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Abstractions.Repository
{
    public interface IDistrictRepository 
    {
        Task<string> Add(clsDistrict district);// return CountryID as String
        Task<bool> Update(clsDistrict district);
        Task<List<DistrictInfo>> GetList();
        Task<bool> IsExist(string Name);
        Task<DistrictInfo> GetById(string param, enDistrictParamType paramType);
    }
}
