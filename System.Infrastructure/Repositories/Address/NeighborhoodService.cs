using Microsoft.EntityFrameworkCore;
using Gridify;
using Gridify.EntityFramework;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Application.Services.Address.Neighborhood.Query;
using System.Domain.Entities;
using System.Domain.Enums.Address;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Address
{
    public class NeighborhoodService : INeighborhoodRepository
    {
        private static readonly IGridifyMapper<NeighborhoodInfo> NeighborhoodFilterMapper =
            new GridifyMapper<NeighborhoodInfo>()
                .AddMap(nameof(NeighborhoodInfo.NeighborhoodName), item => item.NeighborhoodName)
                .AddMap(nameof(NeighborhoodInfo.DistrictName), item => item.DistrictName)
                .AddMap(nameof(NeighborhoodInfo.CityName), item => item.CityName)
                .AddMap(nameof(NeighborhoodInfo.CountryName), item => item.CountryName)
                .AddMap(nameof(NeighborhoodInfo.DistrictID), item => item.DistrictID)
                .AddMap(nameof(NeighborhoodInfo.CityID), item => item.CityID)
                .AddMap(nameof(NeighborhoodInfo.CountryID), item => item.CountryID);

        private readonly AppDbContext _context;
        private readonly DbSet<clsNeighborhood> _dbSet;
        public NeighborhoodService( AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<clsNeighborhood>();
        }

        public async Task<string> Add(clsNeighborhood Neighborhood)
        {
           await _dbSet.AddAsync(Neighborhood);
            return Neighborhood.Id.ToString();
        }

        public async Task<NeighborhoodInfo?> GetById(string param, enNeighborhoodParamType paramType)
        {
            if (enNeighborhoodParamType.ID == paramType)
                return await _dbSet.Where(n => n.Id == clsUtil.ToGuid(param)).
                    Select(n => new NeighborhoodInfo
                    {
                         NeighborhoodID = n.Id.ToString(),
                         NeighborhoodName = n.NeighborhoodName,
                         DistrictID = n.DistrictID.ToString(),
                         DistrictName = n.District.DistrictName,
                         CityID = n.District.CityID.ToString(),
                         CityName = n.District.City.CityName,
                         CountryID = n.District.City.CountryID.ToString(),
                         CountryName = n.District.City.Country.CountryName,
                    }).FirstOrDefaultAsync();
            //enNeighborhoodParamType.Name
            return await _dbSet.Where(n => n.NeighborhoodName == param).
         Select(n => new NeighborhoodInfo
         {
              NeighborhoodID = n.Id.ToString(),
              NeighborhoodName = n.NeighborhoodName,
              DistrictID = n.DistrictID.ToString(),
              DistrictName = n.District.DistrictName,
              CityID = n.District.CityID.ToString(),
              CityName = n.District.City.CityName,
              CountryID = n.District.City.CountryID.ToString(),
              CountryName = n.District.City.Country.CountryName,
         }).FirstOrDefaultAsync();
        }

        public async Task<List<NeighborhoodInfo>> GetList()
        {
            return await _dbSet.Select(n => new NeighborhoodInfo
            {
                 NeighborhoodID = n.Id.ToString(),
                 NeighborhoodName = n.NeighborhoodName,
                 DistrictID = n.DistrictID.ToString(),
                 DistrictName = n.District.DistrictName,
                 CityID = n.District.CityID.ToString(),
                 CityName = n.District.City.CityName,
                 CountryID = n.District.City.CountryID.ToString(),
                 CountryName = n.District.City.Country.CountryName,
             }).ToListAsync();
        }

        public async Task<Paging<NeighborhoodInfo>> GetListFilter(GetAllNeighborhoodQuery request)
        {
            var query = _dbSet.AsQueryable();
            if (request.DistrictID.HasValue)
                query = query.Where(item => item.DistrictID == request.DistrictID.Value);

            return await query
                .Select(n => new NeighborhoodInfo
                {
                    NeighborhoodID = n.Id.ToString(),
                    NeighborhoodName = n.NeighborhoodName,
                    DistrictID = n.DistrictID.ToString(),
                    DistrictName = n.District.DistrictName,
                    CityID = n.District.CityID.ToString(),
                    CityName = n.District.City.CityName,
                    CountryID = n.District.City.CountryID.ToString(),
                    CountryName = n.District.City.Country.CountryName,
                })
                .GridifyAsync(request, NeighborhoodFilterMapper);
        }

        public async Task<bool> IsNameExist(string name, Guid? excludeNeighborhoodID = null)
        {
            return await _dbSet.AnyAsync(neighborhood =>
                neighborhood.NeighborhoodName == name &&
                (!excludeNeighborhoodID.HasValue || neighborhood.Id != excludeNeighborhoodID.Value));
        }

        public Task<bool> Update(clsNeighborhood Neighborhood)
        {
            _dbSet.Update(Neighborhood);
            return Task.FromResult(true);
        }
    }
}
