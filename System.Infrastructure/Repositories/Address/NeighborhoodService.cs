using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Repository;
using System.Application.Abstractions.UOW;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Address
{
    public class NeighborhoodService : INeighborhoodRepository
    {
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

        public async Task<NeighborhoodInfo> GetById(string param, enNeighborhoodParamType paramType)
        {
            if (enNeighborhoodParamType.ID == paramType)
                return await _dbSet.Where(n => n.Id == clsUtil.ToGuid(param)).
                    Select(n => new NeighborhoodInfo
                    {
                        NeighborhoodID = n.Id.ToString(),
                        NeighborhoodName = n.NeighborhoodName,
                        DistrictID = n.DistrictID.ToString(),
                        DistrictName = n.District.DistrictName
                    }).FirstOrDefaultAsync();
            //enNeighborhoodParamType.Name
            return await _dbSet.Where(n => n.NeighborhoodName == param).
         Select(n => new NeighborhoodInfo
         {
             NeighborhoodID = n.Id.ToString(),
             NeighborhoodName = n.NeighborhoodName,
             DistrictID = n.DistrictID.ToString(),
             DistrictName = n.District.DistrictName
         }).FirstOrDefaultAsync();
        }

        public async Task<List<NeighborhoodInfo>> GetList()
        {
            return await _dbSet.Select(n => new NeighborhoodInfo
            {
                NeighborhoodID = n.Id.ToString(),
                NeighborhoodName = n.NeighborhoodName,
                DistrictID = n.DistrictID.ToString(),
                DistrictName = n.District.DistrictName
            }).ToListAsync();
        }

        public async Task<bool> IsExist(string Name)
        {
            return await _dbSet.AnyAsync(c=>c.NeighborhoodName == Name);
        }

        public async Task<bool> Update(clsNeighborhood Neighborhood)
        {
            _dbSet.Update(Neighborhood);
            return  true;
        }
    }
}
