using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Repository;
using System.Application.Services.Address.Address.DTO;
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
        public AddressService(AppDbContext context)
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

        private IQueryable<AddressInfo> ProjectAddressInfo(Guid? addressID = null)
        {
            var query = _dbSet.AsQueryable();
            if (addressID.HasValue)
                query = query.Where(address => address.Id == addressID.Value);

            return query.Select(address => new AddressInfo
            {
                AddressID = address.Id.ToString(),
                CountryID = address.CountryID.ToString(),
                CountryName = address.Country.CountryName,
                CityID = address.CityID.ToString(),
                CityName = address.City.CityName,
                DistrictID = address.DistrictID.ToString(),
                DistrictName = address.District.DistrictName,
                NeighborhoodID = address.NeighborhoodID.ToString(),
                NeighborhoodName = address.Neighborhood.NeighborhoodName,
                Notes = address.Notes,
            });
        }

        private async Task<Guid?> ResolveAddressID(string param, enAddressParamType paramType)
        {
            if (!Guid.TryParse(param, out var ownerID) || ownerID == Guid.Empty)
                return null;

            return paramType switch
            {
                enAddressParamType.ID => ownerID,
                enAddressParamType.PersonID => await _dbPersonSet
                    .Where(person => person.Id == ownerID)
                    .Select(person => person.LivingPlaceID)
                    .FirstOrDefaultAsync(),
                enAddressParamType.InstituteID => await _dbInstituteSet
                    .Where(institute => institute.Id == ownerID)
                    .Select(institute => (Guid?)institute.AddressID)
                    .FirstOrDefaultAsync(),
                _ => null,
            };
        }

        public async Task<AddressInfo?> GetBy(string param, enAddressParamType paramType)
        {
            var addressID = await ResolveAddressID(param, paramType);
            if (!addressID.HasValue || addressID.Value == Guid.Empty)
                return null;

            return await ProjectAddressInfo(addressID).FirstOrDefaultAsync();
        }

        public async Task<clsAddress?> GetEntityBy(string param, enAddressParamType paramType)
        {
            var addressID = await ResolveAddressID(param, paramType);
            if (!addressID.HasValue || addressID.Value == Guid.Empty)
                return null;

            return await _dbSet.FirstOrDefaultAsync(address => address.Id == addressID.Value);
        }

        public Task<bool> Update(clsAddress address)
        {
            _dbSet.Update(address);
            return Task.FromResult(true);
        }
    }
}
