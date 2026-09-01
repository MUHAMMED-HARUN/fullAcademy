

using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Abstractions.Repository
{
    public interface ICountryRepository 
    {
        Task<string> Add(clsCountry country);// return CountryID as String
        Task<bool> Update(clsCountry country );
        Task<List<CountryInfo>> GetList();
        Task<bool> IsExist(string Name);
        Task<CountryInfo> GetById(string param,enCountryParamType paramType);


    }
}
