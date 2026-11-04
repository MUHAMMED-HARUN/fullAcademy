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
        private static readonly IGridifyMapper<TeacherInfo> TeacherFilterMapper =
            new GridifyMapper<TeacherInfo>()
                .AddMap(nameof(TeacherInfo.FirstName), teacher => teacher.FirstName)
                .AddMap(nameof(TeacherInfo.LastName), teacher => teacher.LastName)
                .AddMap(nameof(TeacherInfo.NationalNumber), teacher => teacher.NationalNumber);

        private readonly DbSet<clsTeacher> _dbSet;

        public TeacherService(AppDbContext appDbContext)
        {
            _dbSet = appDbContext.Set<clsTeacher>();
        }

        private static IQueryable<TeacherInfo> ProjectTeacherInfo(
            IQueryable<clsTeacher> teachers)
        {
            return teachers.Select(teacher => new TeacherInfo
            {
                TeacherID = teacher.Id.ToString(),
                PersonID = teacher.Person.Id.ToString(),
                FirstName = teacher.Person.FirstName,
                LastName = teacher.Person.LastName,
                FatherName = teacher.Person.FatherName,
                MotherName = teacher.Person.MotherName,
                MotherLastName = teacher.Person.MotherLastName,
                Gendor = teacher.Person.Gendor,
                BirthDate = teacher.Person.BirthDate,
                NationalNumber = teacher.Person.NationalNumber,
                LivingPlace = teacher.Person.LivingPlace == null
                    ? string.Empty
                    : teacher.Person.LivingPlace.Country.CountryName + " " +
                      teacher.Person.LivingPlace.City.CityName + " " +
                      teacher.Person.LivingPlace.District.DistrictName + " " +
                      teacher.Person.LivingPlace.Neighborhood.NeighborhoodName + " " +
                      teacher.Person.LivingPlace.Notes,
                PlaceOfBirth = teacher.Person.PlaceOfBirth == null
                    ? string.Empty
                    : teacher.Person.PlaceOfBirth.Country.CountryName + " " +
                      teacher.Person.PlaceOfBirth.CityName,
            });
        }

        private IQueryable<TeacherInfo> ProjectActiveTeachers() =>
            ProjectTeacherInfo(_dbSet.Where(teacher =>
                teacher.DeletedBy == null && teacher.Person.DeletedBy == null));

        public async Task<string> Add(clsTeacher entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<Paging<TeacherInfo>> GetAllTeacherFilter(GetAllTeacherQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            return await ProjectActiveTeachers()
                .GridifyAsync(request, TeacherFilterMapper);
        }

        public async Task<TeacherInfo?> GetBy(string param, enTeacherParamType paramType)
        {
            var teachers = _dbSet.Where(teacher =>
                teacher.DeletedBy == null && teacher.Person.DeletedBy == null);

            switch (paramType)
            {
                case enTeacherParamType.ID:
                    if (!TryGetID(param, out var teacherID)) return null;
                    teachers = teachers.Where(teacher => teacher.Id == teacherID);
                    break;
                case enTeacherParamType.PersonID:
                    if (!TryGetID(param, out var personID)) return null;
                    teachers = teachers.Where(teacher => teacher.PersonID == personID);
                    break;
                case enTeacherParamType.NationalNumber:
                    teachers = teachers.Where(teacher => teacher.Person.NationalNumber == param);
                    break;
                default:
                    return null;
            }

            return await ProjectTeacherInfo(teachers).FirstOrDefaultAsync();
        }

        public async Task<clsTeacher?> GetEntityBy(string param, enTeacherParamType paramType)
        {
            var teachers = _dbSet.Where(teacher => teacher.DeletedBy == null);

            switch (paramType)
            {
                case enTeacherParamType.ID:
                    if (!TryGetID(param, out var teacherID)) return null;
                    teachers = teachers.Where(teacher => teacher.Id == teacherID);
                    break;
                case enTeacherParamType.PersonID:
                    if (!TryGetID(param, out var personID)) return null;
                    teachers = teachers.Where(teacher => teacher.PersonID == personID);
                    break;
                case enTeacherParamType.NationalNumber:
                    teachers = teachers.Where(teacher => teacher.Person.NationalNumber == param);
                    break;
                default:
                    return null;
            }

            return await teachers.FirstOrDefaultAsync();
        }

        public async Task<List<TeacherInfo>> GetList()
        {
            return await ProjectActiveTeachers().ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enTeacherParamType paramType)
        {
            return paramType switch
            {
                enTeacherParamType.ID => TryGetID(param, out var teacherID) &&
                    await _dbSet.AnyAsync(teacher => teacher.Id == teacherID),
                enTeacherParamType.PersonID => TryGetID(param, out var personID) &&
                    await _dbSet.AnyAsync(teacher => teacher.PersonID == personID),
                enTeacherParamType.NationalNumber =>
                    await _dbSet.AnyAsync(teacher => teacher.Person.NationalNumber == param),
                _ => false,
            };
        }

        public Task<bool> IsTeacher(string param, enTeacherParamType paramType) =>
            IsExistBy(param, paramType);

        public Task<bool> Update(clsTeacher entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(true);
        }

        private static bool TryGetID(string value, out Guid id) =>
            Guid.TryParseExact(value, "D", out id) && id != Guid.Empty;
    }
}
