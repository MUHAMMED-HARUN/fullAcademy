

using Gridify;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.Country.Query;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Abstractions.Repository
{
    public interface ICountryRepository 
    {
        Task<string> Add(clsCountry country);// return CountryID as String
        Task<bool> Update(clsCountry country );
        Task<List<CountryInfo>> GetList();
        Task<Paging<CountryInfo>> GetListFilter(GetAllCountryQuery request);
        Task<bool> IsNameExist(string name, Guid? excludeCountryID = null);
        Task<bool> IsCodeExist(string code, Guid? excludeCountryID = null);
        Task<CountryInfo?> GetById(string param,enCountryParamType paramType);


    }
}
