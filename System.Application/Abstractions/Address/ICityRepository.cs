using Gridify;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.City.Query;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Abstractions.Repository
{
    public interface ICityRepository
    {
        Task<string> Add(clsCity City);// return CountryID as String
        Task<bool> Update(clsCity City);
        Task<List<CityInfo>> GetList();
        Task<Paging<CityInfo>> GetListFilter(GetAllCityQuery request);
        Task<bool> IsExist(string Name);
        Task<CityInfo?> GetById(string param, enCityParamType paramType);
    }
}
