using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Person.Person.DTO;
using System.Application.Services.Person.Person.Query;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Person
{
    public class PersonService : IPersonRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsPerson> _dbSet;

        public PersonService(AppDbContext appDbContext )
        {
         _context = appDbContext;
            _dbSet = _context.Set<clsPerson>();
        }

        public async Task<string> Add(clsPerson entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<PersonInfo> GetBy(string param, enPersonParamType paramType)
        {
            if (paramType == enPersonParamType.ID)

        return   await _dbSet.Where(x => x.Id == clsUtil.ToGuid(param)).
                     Select(x => new PersonInfo
                     {
                         PersonID = x.Id.ToString(),
                         FirstName = x.FirstName,
                         LastName = x.LastName,
                         FatherName = x.FatherName,
                         MotherName = x.MotherName,
                         MotherLastName = x.MotherLastName,
                         Gendor = x.Gendor,
                         BirthDate = x.BirthDate,
                         NationalNumber = x.NationalNumber,
                         LivingPlace = x.LivingPlace.Country.CountryName + " " + x.LivingPlace.City.CityName + " " +
                         x.LivingPlace.District.DistrictName + " " + x.LivingPlace.Neighborhood.NeighborhoodName + " " + x.LivingPlace.Notes,
                         PlaceOfBirth = x.PlaceOfBirth.Country.CountryName + " " + x.PlaceOfBirth.CityName,
                     }
                    ).FirstOrDefaultAsync();
      
            else
                return await _dbSet.Where(x => x.NationalNumber == param ).
                Select(x => new PersonInfo
                {
                    PersonID = x.Id.ToString(),
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName,
                    MotherName = x.MotherName,
                    MotherLastName = x.MotherLastName,
                    Gendor = x.Gendor,
                    BirthDate = x.BirthDate,
                    NationalNumber = x.NationalNumber,
                    LivingPlace = x.LivingPlace.Country.CountryName + " " + x.LivingPlace.City.CityName + " " +
                    x.LivingPlace.District.DistrictName + " " + x.LivingPlace.Neighborhood.NeighborhoodName + " " + x.LivingPlace.Notes,
                    PlaceOfBirth = x.PlaceOfBirth.Country.CountryName + " " + x.PlaceOfBirth.CityName,
                }
               ).FirstOrDefaultAsync();
        }

        public async Task<clsPerson> GetEntityBy(string param, enPersonParamType paramType)
        {
            if(paramType == enPersonParamType.ID)
            return await _dbSet.Where(x => x.Id == clsUtil.ToGuid(param)).AsNoTracking().FirstOrDefaultAsync();
          

                return await _dbSet.Where(x => x.NationalNumber == param).FirstOrDefaultAsync();
        }

        public async Task<List<PersonInfo>> GetList()
        {
            return await _dbSet.Where(x=>x.DeletedBy ==null).
      Select(x => new PersonInfo
      {
          PersonID = x.Id.ToString(),
          FirstName = x.FirstName,
          LastName = x.LastName,
          FatherName = x.FatherName,
          MotherName = x.MotherName,
          MotherLastName = x.MotherLastName,
          Gendor = x.Gendor,
          BirthDate = x.BirthDate,
          NationalNumber = x.NationalNumber,
          LivingPlace = x.LivingPlace.Country.CountryName + " " + x.LivingPlace.City.CityName + " " +
          x.LivingPlace.District.DistrictName + " " + x.LivingPlace.Neighborhood.NeighborhoodName + " " + x.LivingPlace.Notes,
          PlaceOfBirth = x.PlaceOfBirth.Country.CountryName + " " + x.PlaceOfBirth.CityName,
      }
     ).ToListAsync();
        }

        public async Task<Paging<PersonInfo>> GetListFilter(GetAllPersonQuery request)
        {
            return await _dbSet.
   Select(x => new PersonInfo
   {
       PersonID = x.Id.ToString(),
       FirstName = x.FirstName,
       LastName = x.LastName,
       FatherName = x.FatherName,
       MotherName = x.MotherName,
       MotherLastName = x.MotherLastName,
       Gendor = x.Gendor,
       BirthDate = x.BirthDate,
       NationalNumber = x.NationalNumber,
       LivingPlace = x.LivingPlace.Country.CountryName + " " + x.LivingPlace.City.CityName + " " +
       x.LivingPlace.District.DistrictName + " " + x.LivingPlace.Neighborhood.NeighborhoodName + " " + x.LivingPlace.Notes,
       PlaceOfBirth = x.PlaceOfBirth.Country.CountryName + " " + x.PlaceOfBirth.CityName,
   }
  ).GridifyAsync(request);
        }

        public async Task<bool> IsExist(string param, enPersonParamType paramType)
        {
            if (paramType == enPersonParamType.ID)
                return await _dbSet.AnyAsync(x => x.Id == clsUtil.ToGuid(param));

            return await _dbSet.AnyAsync(x => x.NationalNumber == param);
        }

        public async Task<bool> Update(clsPerson entity)
        {
             _dbSet.Update(entity);
            return true;
        }
    }
}
