using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Common;
using System.Application.Services.Address.Address.DTO;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Entities;
using System.Domain.Entities.Address;
using System.Domain.Entities.Institue;
using System.Domain.Entities.Person;
using System.Domain.Enums.Address;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Address
{
    public class AddressService : IAddressRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAddress> _dbSet;
        private readonly DbSet<clsPerson> _dbPersonSet;
        private readonly DbSet<clsInstitute> _dbInstituteSet;
        public AddressService( AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<clsAddress>();
            _dbPersonSet = _context.Set<clsPerson>();
            _dbInstituteSet = _context.Set<clsInstitute>();
        }

        public async Task<string> Add(clsAddress address)
        {
           await _dbSet.AddAsync(address);
            return  address.Id.ToString();   
        }

        public async Task<AddressInfo> GetBy(string param, enAddressParamType paramType)
        {
            if (paramType == enAddressParamType.ID)
                return await _dbSet.Where(a => a.Id == clsUtil.ToGuid(param)).
                    Select(a => new AddressInfo
                    {
                        AddressID = a.Id.ToString(),
                        CountryID = a.CountryID.ToString(),
                        CountryName = a.Country.CountryName,
                        CityID = a.CityID.ToString(),
                        CityName = a.City.CityName,
                        DistrictID = a.DistrictID.ToString(),
                        DistrictName = a.District.DistrictName,
                        NeighborhoodID = a.NeighborhoodID.ToString(),
                        NeighborhoodName = a.Neighborhood.NeighborhoodName,
                        Notes = a.Notes
                    }).FirstOrDefaultAsync();
            else if(paramType == enAddressParamType.PersonID)
            {
              Guid? AddressID= await  _dbPersonSet.Where(p => p.Id == clsUtil.ToGuid(param)).Select(p => p.LivingPlaceID).FirstOrDefaultAsync();
                return await _dbSet.Where(a => a.Id == AddressID.Value).
          Select(a => new AddressInfo
          {
              AddressID = a.Id.ToString(),
              CountryID = a.CountryID.ToString(),
              CountryName = a.Country.CountryName,
              CityID = a.CityID.ToString(),
              CityName = a.City.CityName,
              DistrictID = a.DistrictID.ToString(),
              DistrictName = a.District.DistrictName,
              NeighborhoodID = a.NeighborhoodID.ToString(),
              NeighborhoodName = a.Neighborhood.NeighborhoodName,
              Notes = a.Notes
          }).FirstOrDefaultAsync();
            }

            else  
            {
                Guid? AddressID = await _dbInstituteSet.Where(i => i.Id == clsUtil.ToGuid(param)).Select(i => i.AddressID).FirstOrDefaultAsync();
                return await _dbSet.Where(a => a.Id == AddressID.Value).
          Select(a => new AddressInfo
          {
              AddressID = a.Id.ToString(),
              CountryID = a.CountryID.ToString(),
              CountryName = a.Country.CountryName,
              CityID = a.CityID.ToString(),
              CityName = a.City.CityName,
              DistrictID = a.DistrictID.ToString(),
              DistrictName = a.District.DistrictName,
              NeighborhoodID = a.NeighborhoodID.ToString(),
              NeighborhoodName = a.Neighborhood.NeighborhoodName,
              Notes = a.Notes
          }).FirstOrDefaultAsync();
            }


        }

        public async Task<clsAddress> GetEntityBy(string param, enAddressParamType paramType)
        {
            if (paramType == enAddressParamType.ID)
                return await _dbSet.Where(a => a.Id == clsUtil.ToGuid(param)).
                 FirstOrDefaultAsync();
            else if (paramType == enAddressParamType.PersonID)
            {
                Guid? AddressID = await _dbPersonSet.Where(p => p.Id == clsUtil.ToGuid(param)).Select(p => p.LivingPlaceID).FirstOrDefaultAsync();
                return await _dbSet.Where(a => a.Id == AddressID.Value).
        FirstOrDefaultAsync();
            }

            else
            {
                Guid? AddressID = await _dbInstituteSet.Where(i => i.Id == clsUtil.ToGuid(param)).Select(i => i.AddressID).FirstOrDefaultAsync();
                return await _dbSet.Where(a => a.Id == AddressID.Value).
        FirstOrDefaultAsync();
            }

        }

        public async Task<bool> Update(clsAddress address)
        {
           _dbSet.Update(address);
            return true;
        }
    }
}
