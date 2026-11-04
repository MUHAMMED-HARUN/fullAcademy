

using Gridify;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Application.Services.Address.Neighborhood.Query;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Abstractions.Repository
{
    public interface INeighborhoodRepository
    {
        Task<string> Add(clsNeighborhood Neighborhood);// return CountryID as String
        Task<bool> Update(clsNeighborhood Neighborhood);
        Task<List<NeighborhoodInfo>> GetList();
        Task<Paging<NeighborhoodInfo>> GetListFilter(GetAllNeighborhoodQuery request);
        Task<bool> IsNameExist(string name, Guid? excludeNeighborhoodID = null);
        Task<NeighborhoodInfo?> GetById(string param, enNeighborhoodParamType paramType);
    }
}
