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
        private static readonly IGridifyMapper<PersonInfo> PersonFilterMapper =
            new GridifyMapper<PersonInfo>()
                .AddMap(nameof(PersonInfo.FirstName), person => person.FirstName)
                .AddMap(nameof(PersonInfo.LastName), person => person.LastName)
                .AddMap(nameof(PersonInfo.NationalNumber), person => person.NationalNumber)
                .AddMap(nameof(PersonInfo.Gendor), person => person.Gendor);

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
            var people = _dbSet.Where(person => person.DeletedBy == null);
            if (paramType == enPersonParamType.ID)
                people = people.Where(person => person.Id == clsUtil.ToGuid(param));
            else if (paramType == enPersonParamType.NationalNumber)
                people = people.Where(person => person.NationalNumber == param);
            else
                return null;

            return await ProjectPersonInfo(people).FirstOrDefaultAsync();
        }

        public async Task<PersonForUpsertDto?> GetForUpsert(Guid personID)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(person => person.Id == personID && person.DeletedBy == null)
                .Select(person => new PersonForUpsertDto
                {
                    PersonID = person.Id.ToString(),
                    FirstName = person.FirstName,
                    FatherName = person.FatherName,
                    LastName = person.LastName,
                    MotherName = person.MotherName,
                    MotherLastName = person.MotherLastName,
                    NationalNumber = person.NationalNumber,
                    Gendor = person.Gendor,
                    BirthDate = person.BirthDate,
                    PlaceOfBirthID = person.PlaceOfBirthID.HasValue
                        ? person.PlaceOfBirthID.Value.ToString()
                        : null,
                    LivingPlaceID = person.LivingPlaceID.HasValue
                        ? person.LivingPlaceID.Value.ToString()
                        : null,
                    Address = person.LivingPlace == null
                        ? null
                        : new PersonAddressForUpsertDto
                        {
                            AddressID = person.LivingPlace.Id.ToString(),
                            CountryID = person.LivingPlace.CountryID.ToString(),
                            CityID = person.LivingPlace.CityID.ToString(),
                            DistrictID = person.LivingPlace.DistrictID.ToString(),
                            NeighborhoodID = person.LivingPlace.NeighborhoodID.ToString(),
                            Notes = person.LivingPlace.Notes,
                        },
                })
                .FirstOrDefaultAsync();
        }

        public async Task<clsPerson> GetEntityBy(string param, enPersonParamType paramType)
        {
            var people = _dbSet.Where(person => person.DeletedBy == null);
            if (paramType == enPersonParamType.ID)
                people = people.Where(person => person.Id == clsUtil.ToGuid(param));
            else if (paramType == enPersonParamType.NationalNumber)
                people = people.Where(person => person.NationalNumber == param);
            else
                return null;

            return await people.FirstOrDefaultAsync();
        }

        private static IQueryable<PersonInfo> ProjectPersonInfo(IQueryable<clsPerson> people)
        {
            return people.Select(person => new PersonInfo
            {
                PersonID = person.Id.ToString(),
                FirstName = person.FirstName,
                LastName = person.LastName,
                FatherName = person.FatherName,
                MotherName = person.MotherName,
                MotherLastName = person.MotherLastName,
                Gendor = person.Gendor,
                BirthDate = person.BirthDate,
                NationalNumber = person.NationalNumber,
                LivingPlace = person.LivingPlace == null
                    ? string.Empty
                    : person.LivingPlace.Country.CountryName + " " +
                      person.LivingPlace.City.CityName + " " +
                      person.LivingPlace.District.DistrictName + " " +
                      person.LivingPlace.Neighborhood.NeighborhoodName + " " +
                      person.LivingPlace.Notes,
                PlaceOfBirth = person.PlaceOfBirth == null
                    ? string.Empty
                    : person.PlaceOfBirth.Country.CountryName + " " +
                      person.PlaceOfBirth.CityName,
            });
        }

        public async Task<List<PersonInfo>> GetList()
        {
            return await ProjectPersonInfo(_dbSet.Where(person => person.DeletedBy == null))
                .ToListAsync();
        }

        public async Task<Paging<PersonInfo>> GetListFilter(GetAllPersonQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            return await ProjectPersonInfo(_dbSet.Where(person => person.DeletedBy == null))
                .GridifyAsync(request, PersonFilterMapper);
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
