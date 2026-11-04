using Microsoft.EntityFrameworkCore;
using Gridify;
using Gridify.EntityFramework;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;
using System.Application.Services.Address.District.Query;
using System.Domain.Entities;
using System.Domain.Enums.Address;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Address
{
    public class DistrictService : IDistrictRepository
    {
        private static readonly IGridifyMapper<DistrictInfo> DistrictFilterMapper =
            new GridifyMapper<DistrictInfo>()
                .AddMap(nameof(DistrictInfo.DistrictName), district => district.DistrictName)
                .AddMap(nameof(DistrictInfo.CityName), district => district.CityName)
                .AddMap(nameof(DistrictInfo.CountryName), district => district.CountryName)
                .AddMap(nameof(DistrictInfo.CityID), district => district.CityID)
                .AddMap(nameof(DistrictInfo.CountryID), district => district.CountryID);

        private readonly AppDbContext _context;
        private readonly DbSet<clsDistrict> _dbSet;
        public DistrictService( AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<clsDistrict>();
        }

        public async Task<string> Add(clsDistrict district)
        {
            await _dbSet.AddAsync(district);
            return district.Id.ToString();
        }

        public async Task<DistrictInfo?> GetById(string param, enDistrictParamType paramType)
        {
            if (enDistrictParamType.ID == paramType)
                return await _dbSet.Where(d => d.Id == clsUtil.ToGuid(param)).
                    Select(d => new DistrictInfo
                    {
                        ID = d.Id.ToString(),
                        DistrictName = d.DistrictName,
                        CityID = d.CityID.ToString(),
                        CityName = d.City.CityName,
                        CountryID = d.City.CountryID.ToString(),
                        CountryName = d.City.Country.CountryName,
                    }).FirstOrDefaultAsync();


            //enDistrictParamType.Name == paramType
            return await _dbSet.Where(d => d.DistrictName == param).
   Select(d => new DistrictInfo
   {
       ID = d.Id.ToString(),
       DistrictName = d.DistrictName,
       CityID = d.CityID.ToString(),
       CityName = d.City.CityName,
       CountryID = d.City.CountryID.ToString(),
       CountryName = d.City.Country.CountryName,
   }).FirstOrDefaultAsync();


        }

        public async Task<List<DistrictInfo>> GetList()
        {
            return await _dbSet.Select(d => new DistrictInfo
            {
                ID = d.Id.ToString(),
                DistrictName = d.DistrictName,
                CityID = d.CityID.ToString(),
                CityName = d.City.CityName,
                CountryID = d.City.CountryID.ToString(),
                CountryName = d.City.Country.CountryName,
            }).ToListAsync();
        }

        public async Task<Paging<DistrictInfo>> GetListFilter(GetAllDistrictQuery request)
        {
            var query = _dbSet.AsQueryable();
            if (request.CityID.HasValue)
                query = query.Where(district => district.CityID == request.CityID.Value);

            return await query
                .Select(district => new DistrictInfo
                {
                    ID = district.Id.ToString(),
                    DistrictName = district.DistrictName,
                    CityID = district.CityID.ToString(),
                    CityName = district.City.CityName,
                    CountryID = district.City.CountryID.ToString(),
                    CountryName = district.City.Country.CountryName,
                })
                .GridifyAsync(request, DistrictFilterMapper);
        }

        public async Task<bool> IsNameExist(string name, Guid? excludeDistrictID = null)
        {
            return await _dbSet.AnyAsync(district =>
                district.DistrictName == name &&
                (!excludeDistrictID.HasValue || district.Id != excludeDistrictID.Value));
        }

        public Task<bool> Update(clsDistrict district)
        {
            _dbSet.Update(district);
            return Task.FromResult(true);
        }
    }
}
