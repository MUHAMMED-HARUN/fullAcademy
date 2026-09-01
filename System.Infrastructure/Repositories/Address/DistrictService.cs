using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Address
{
    public class DistrictService : IDistrictRepository
    {
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

        public async Task<DistrictInfo> GetById(string param, enDistrictParamType paramType)
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

        public async Task<bool> IsExist(string Name)
        {
            return await _dbSet.AnyAsync(d => d.DistrictName == Name);
        }

        public async Task<bool> Update(clsDistrict district)
        {
            _dbSet.Update(district);
            return true;
        }
    }
}
