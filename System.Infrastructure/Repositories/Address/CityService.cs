using Microsoft.EntityFrameworkCore;
using Gridify;
using Gridify.EntityFramework;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.City.Query;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Address
{
    public class CityService : ICityRepository
    {
        private static readonly IGridifyMapper<CityInfo> CityFilterMapper =
            new GridifyMapper<CityInfo>()
                .AddMap(nameof(CityInfo.Name), city => city.Name)
                .AddMap(nameof(CityInfo.Code), city => city.Code)
                .AddMap(nameof(CityInfo.CountryName), city => city.CountryName)
                .AddMap(nameof(CityInfo.CountryID), city => city.CountryID);

        private readonly AppDbContext _context;
        private readonly DbSet<clsCity> _dbSet;
        public CityService( AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<clsCity>();
        }

        public async Task<string> Add(clsCity City)
        {
           await _dbSet.AddAsync(City);
            return City.Id.ToString();
        }

        public async Task<CityInfo?> GetById(string param, enCityParamType paramType)
        {
            if (enCityParamType.ID == paramType)
                return await _dbSet.Where(c => c.Id == clsUtil.ToGuid(param)).
                    Select(c => new CityInfo
                    {
                        ID = c.Id.ToString(),
                        Name = c.CityName,
                        Code = c.CityCode,
                        CountryID = c.CountryID.ToString(),
                        CountryName = c.Country.CountryName
                    }).FirstOrDefaultAsync();

            if (enCityParamType.Name == paramType)
                return await _dbSet.Where(c => c.CityName == param).
                    Select(c => new CityInfo
                    {
                        ID = c.Id.ToString(),
                        Name = c.CityName,
                        Code = c.CityCode,
                        CountryID = c.CountryID.ToString(),
                        CountryName = c.Country.CountryName
                    }).FirstOrDefaultAsync();

            //enCityParamType.Code == paramType
            return await _dbSet.Where(c => c.CityCode == param).
                 Select(c => new CityInfo
                {
                    ID = c.Id.ToString(),
                    Name = c.CityName,
                    Code = c.CityCode,
                    CountryID = c.CountryID.ToString(),
                    CountryName = c.Country.CountryName
                }).FirstOrDefaultAsync();
        }

        public async Task<List<CityInfo>> GetList()
        {
           return await _dbSet.Select(c => new CityInfo
           {
               ID = c.Id.ToString(),
               Name = c.CityName,
               Code = c.CityCode,
               CountryID = c.CountryID.ToString(),
               CountryName = c.Country.CountryName
           }).ToListAsync();
        }

        public async Task<Paging<CityInfo>> GetListFilter(GetAllCityQuery request)
        {
            return await _dbSet
                .Select(city => new CityInfo
                {
                    ID = city.Id.ToString(),
                    Name = city.CityName,
                    Code = city.CityCode,
                    CountryID = city.CountryID.ToString(),
                    CountryName = city.Country.CountryName,
                })
                .GridifyAsync(request, CityFilterMapper);
        }

        public async Task<bool> IsExist(string Name)
        {
            return await _dbSet.AnyAsync(c => c.CityName == Name);
        }

        public Task<bool> Update(clsCity City)
        {
            _dbSet.Update(City);
            return Task.FromResult(true);
        }
    }
}
