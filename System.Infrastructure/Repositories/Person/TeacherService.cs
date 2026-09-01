using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Person.Teacher.DTO;
using System.Application.Services.Person.Teacher.Query;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Person
{
    public class TeacherService : ITeacherRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsTeacher> _dbSet;

        public TeacherService(AppDbContext appDbContext )
        {
         _context = appDbContext;
            _dbSet = _context.Set<clsTeacher>();
        }

        public async Task<string> Add(clsTeacher entity)
        {
             await _dbSet.AddAsync( entity );
            return entity.Id.ToString();
        }

        public async Task<Paging<TeacherInfo>> GetAllTeacherFilter(GetAllTeacherQuery requset)
        {
            return await _dbSet.Select(x => new TeacherInfo
            {
                TeacherID = x.Id.ToString(),
                PersonID = x.Person.Id.ToString(),
                FirstName = x.Person.FirstName,
                LastName = x.Person.LastName,
                FatherName = x.Person.FatherName,
                MotherName = x.Person.MotherName,
                MotherLastName = x.Person.MotherLastName,
                Gendor = x.Person.Gendor,
                BirthDate = x.Person.BirthDate,
                NationalNumber = x.Person.NationalNumber,
                LivingPlace = x.Person.LivingPlace.Country.CountryName + " " + x.Person.LivingPlace.City.CityName + " " +
                                    x.Person.LivingPlace.District.DistrictName + " " + x.Person.LivingPlace.Neighborhood.NeighborhoodName + " " + x.Person.LivingPlace.Notes,
                PlaceOfBirth = x.Person.PlaceOfBirth.Country.CountryName + " " + x.Person.PlaceOfBirth.CityName,
            }
                               ).GridifyAsync(requset);
        }

        public async Task<TeacherInfo> GetBy(string param, enTeacherParamType paramType)
        {
            if (paramType == enTeacherParamType.ID)

                return await _dbSet.Where(x => x.Id == clsUtil.ToGuid(param)).
                             Select(x => new TeacherInfo
                             {
                                 TeacherID= x.Id.ToString(),
                                 PersonID = x.Person.Id.ToString(),
                                 FirstName = x.Person.FirstName,
                                 LastName = x.Person.LastName,
                                 FatherName = x.Person.FatherName,
                                 MotherName = x.Person.MotherName,
                                 MotherLastName = x.Person.MotherLastName,
                                 Gendor = x.Person.Gendor,
                                 BirthDate = x.Person.BirthDate,
                                 NationalNumber = x.Person.NationalNumber,
                                 LivingPlace = x.Person.LivingPlace.Country.CountryName + " " + x.Person.LivingPlace.City.CityName + " " +
                                 x.Person.LivingPlace.District.DistrictName + " " + x.Person.LivingPlace.Neighborhood.NeighborhoodName + " " + x.Person.LivingPlace.Notes,
                                 PlaceOfBirth = x.Person.PlaceOfBirth.Country.CountryName + " " + x.Person.PlaceOfBirth.CityName,
                             }
                            ).FirstOrDefaultAsync();

            else if(paramType == enTeacherParamType.PersonID)
                return await _dbSet.Where(x => x.PersonID == clsUtil.ToGuid(param)).
                         Select(x => new TeacherInfo
                         {
                             TeacherID = x.Id.ToString(),
                             PersonID = x.Person.Id.ToString(),
                             FirstName = x.Person.FirstName,
                             LastName = x.Person.LastName,
                             FatherName = x.Person.FatherName,
                             MotherName = x.Person.MotherName,
                             MotherLastName = x.Person.MotherLastName,
                             Gendor = x.Person.Gendor,
                             BirthDate = x.Person.BirthDate,
                             NationalNumber = x.Person.NationalNumber,
                             LivingPlace = x.Person.LivingPlace.Country.CountryName + " " + x.Person.LivingPlace.City.CityName + " " +
                                 x.Person.LivingPlace.District.DistrictName + " " + x.Person.LivingPlace.Neighborhood.NeighborhoodName + " " + x.Person.LivingPlace.Notes,
                             PlaceOfBirth = x.Person.PlaceOfBirth.Country.CountryName + " " + x.Person.PlaceOfBirth.CityName,
                         }
                            ).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(x => x.Person.NationalNumber == param).
                      Select(x => new TeacherInfo
                      {
                          TeacherID = x.Id.ToString(),
                          PersonID = x.Person.Id.ToString(),
                          FirstName = x.Person.FirstName,
                          LastName = x.Person.LastName,
                          FatherName = x.Person.FatherName,
                          MotherName = x.Person.MotherName,
                          MotherLastName = x.Person.MotherLastName,
                          Gendor = x.Person.Gendor,
                          BirthDate = x.Person.BirthDate,
                          NationalNumber = x.Person.NationalNumber,
                          LivingPlace = x.Person.LivingPlace.Country.CountryName + " " + x.Person.LivingPlace.City.CityName + " " +
                              x.Person.LivingPlace.District.DistrictName + " " + x.Person.LivingPlace.Neighborhood.NeighborhoodName + " " + x.Person.LivingPlace.Notes,
                          PlaceOfBirth = x.Person.PlaceOfBirth.Country.CountryName + " " + x.Person.PlaceOfBirth.CityName,
                      }
                         ).FirstOrDefaultAsync();
        }

        public async Task<clsTeacher> GetEntityBy(string param, enTeacherParamType paramType)
        {
            if (paramType == enTeacherParamType.PersonID)
                return await _dbSet.Where(x => x.PersonID == clsUtil.ToGuid(param)).AsNoTracking().FirstOrDefaultAsync();
            else if (paramType == enTeacherParamType.ID)
                return await _dbSet.Where(x => x.Id == clsUtil.ToGuid(param)).AsNoTracking().FirstOrDefaultAsync();

                return await _dbSet.Where(x => x.Person.NationalNumber == param).AsNoTracking().FirstOrDefaultAsync();

        }

        public async Task<List<TeacherInfo>> GetList()
        {
         return await   _dbSet.Select(x => new TeacherInfo
            {
                TeacherID = x.Id.ToString(),
                PersonID = x.Person.Id.ToString(),
                FirstName = x.Person.FirstName,
                LastName = x.Person.LastName,
                FatherName = x.Person.FatherName,
                MotherName = x.Person.MotherName,
                MotherLastName = x.Person.MotherLastName,
                Gendor = x.Person.Gendor,
                BirthDate = x.Person.BirthDate,
                NationalNumber = x.Person.NationalNumber,
                LivingPlace = x.Person.LivingPlace.Country.CountryName + " " + x.Person.LivingPlace.City.CityName + " " +
                          x.Person.LivingPlace.District.DistrictName + " " + x.Person.LivingPlace.Neighborhood.NeighborhoodName + " " + x.Person.LivingPlace.Notes,
                PlaceOfBirth = x.Person.PlaceOfBirth.Country.CountryName + " " + x.Person.PlaceOfBirth.CityName,
            }
                     ).ToListAsync();
            }

        public async Task<bool> IsExistBy(string param, enTeacherParamType paramType)
        {
            if (paramType == enTeacherParamType.PersonID)
                return await _dbSet.AnyAsync(x => x.PersonID == clsUtil.ToGuid(param));
            else if (paramType == enTeacherParamType.ID)
                return await _dbSet.AnyAsync(x => x.Id == clsUtil.ToGuid(param));

            return await _dbSet.AnyAsync(x => x.Person.NationalNumber == param);
        }

        public async Task<bool> IsTeacher(string param, enTeacherParamType paramType)
        {
            if (paramType == enTeacherParamType.PersonID)
                return await _dbSet.AnyAsync(x => x.PersonID == clsUtil.ToGuid(param));

            return await _dbSet.AnyAsync(x => x.Person.NationalNumber == param);
        }

        public async Task<bool> Update(clsTeacher entity)
        {
            _dbSet.Update(entity);
            return true;
        }
    }
}
