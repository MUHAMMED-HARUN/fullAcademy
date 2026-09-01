using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Address
{
    public class CountryService : ICountryRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsCountry> _dbSet;
        public CountryService( AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<clsCountry>();
        }
        public async Task<string> Add(clsCountry country)
        {
            await _dbSet.AddAsync(country);
            return country.Id.ToString();
        }

        public async Task<CountryInfo> GetById(string param, enCountryParamType paramType)
        {
            if (enCountryParamType.ID == paramType)
                return await _dbSet.Where(c => c.Id == clsUtil.ToGuid(param)).
                     Select(c => new CountryInfo { ID = c.Id.ToString(), Name = c.CountryName, Code = c.CountryCode }).
                    FirstOrDefaultAsync();

            if (paramType == enCountryParamType.Name)
                return await _dbSet.Where(c => c.CountryName == param).
                     Select(c => new CountryInfo { ID = c.Id.ToString(), Name = c.CountryName, Code = c.CountryCode }).
                    FirstOrDefaultAsync();

        //    paramType==enCountryParamType.Code
                return await _dbSet.Where(c=>c.CountryCode == param).
                     Select(c => new CountryInfo { ID = c.Id.ToString(), Name = c.CountryName, Code = c.CountryCode }).
                    FirstOrDefaultAsync();
        }

        public async Task<List<CountryInfo>> GetList()
        {
            return await _dbSet.Select(c => new CountryInfo { ID = c.Id.ToString(), Name = c.CountryName, Code = c.CountryCode }).ToListAsync();
        }

        public async Task<bool> IsExist(string Name)
        {
            return await _dbSet.AnyAsync(c=>c.CountryName== Name);
        }

        public  async Task<bool> Update(clsCountry country)
        {
            _dbSet.Update(country);
            return true;
        }
    }
}
