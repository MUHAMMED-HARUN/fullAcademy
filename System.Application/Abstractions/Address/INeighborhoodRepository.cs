

using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Abstractions.Repository
{
    public interface INeighborhoodRepository
    {
        Task<string> Add(clsNeighborhood Neighborhood);// return CountryID as String
        Task<bool> Update(clsNeighborhood Neighborhood);
        Task<List<NeighborhoodInfo>> GetList();
        Task<bool> IsExist(string Name);
        Task<NeighborhoodInfo> GetById(string param, enNeighborhoodParamType paramType);
    }
}
