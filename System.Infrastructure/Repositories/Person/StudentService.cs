using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Person.Student.DTO;
using System.Application.Services.Person.Student.Query;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Person
{
    public class StudentService : IStudentRepository
    {
        private static readonly IGridifyMapper<StudentInfo> StudentFilterMapper =
            new GridifyMapper<StudentInfo>()
                .AddMap(nameof(StudentInfo.FirstName), student => student.FirstName)
                .AddMap(nameof(StudentInfo.NationalNumber), student => student.NationalNumber)
                .AddMap(nameof(StudentInfo.Gendor), student => student.Gendor)
                .AddMap("BirthDateFrom", student => student.BirthDate)
                .AddMap("BirthDateTo", student => student.BirthDate);

        private readonly DbSet<clsStudent> _dbSet;

        public StudentService(AppDbContext appDbContext)
        {
            _dbSet = appDbContext.Set<clsStudent>();
        }

        private static IQueryable<StudentInfo> ProjectStudentInfo(
            IQueryable<clsStudent> students)
        {
            return students.Select(student => new StudentInfo
                {
                    StudentID = student.Id.ToString(),
                    PersonID = student.Person.Id.ToString(),
                    FirstName = student.Person.FirstName,
                    LastName = student.Person.LastName,
                    FatherName = student.Person.FatherName,
                    MotherName = student.Person.MotherName,
                    MotherLastName = student.Person.MotherLastName,
                    Gendor = student.Person.Gendor,
                    BirthDate = student.Person.BirthDate,
                    NationalNumber = student.Person.NationalNumber,
                    LivingPlace = student.Person.LivingPlace == null
                        ? string.Empty
                        : student.Person.LivingPlace.Country.CountryName + " " +
                          student.Person.LivingPlace.City.CityName + " " +
                          student.Person.LivingPlace.District.DistrictName + " " +
                          student.Person.LivingPlace.Neighborhood.NeighborhoodName + " " +
                          student.Person.LivingPlace.Notes,
                    PlaceOfBirth = student.Person.PlaceOfBirth == null
                        ? string.Empty
                        : student.Person.PlaceOfBirth.Country.CountryName + " " +
                          student.Person.PlaceOfBirth.CityName,
                });
        }

        private IQueryable<StudentInfo> ProjectActiveStudents() =>
            ProjectStudentInfo(_dbSet.Where(student =>
                student.DeletedBy == null && student.Person.DeletedBy == null));

        public async Task<string> Add(clsStudent entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<Paging<StudentInfo>> GetAllStudentFilter(GetAllStudentQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            return await ProjectActiveStudents()
                .GridifyAsync(request, StudentFilterMapper);
        }

        public async Task<StudentInfo?> GetBy(string param, enStudentParamType paramType)
        {
            var students = _dbSet.Where(student =>
                student.DeletedBy == null && student.Person.DeletedBy == null);

            switch (paramType)
            {
                case enStudentParamType.ID:
                    if (!TryGetID(param, out var studentID)) return null;
                    students = students.Where(student => student.Id == studentID);
                    break;
                case enStudentParamType.PersonID:
                    if (!TryGetID(param, out var personID)) return null;
                    students = students.Where(student => student.PersonID == personID);
                    break;
                case enStudentParamType.NationalNumber:
                    students = students.Where(student => student.Person.NationalNumber == param);
                    break;
                default:
                    return null;
            }

            return await ProjectStudentInfo(students).FirstOrDefaultAsync();
        }

        public async Task<clsStudent?> GetEntityBy(string param, enStudentParamType paramType)
        {
            var students = _dbSet.Where(student => student.DeletedBy == null);

            switch (paramType)
            {
                case enStudentParamType.ID:
                    if (!TryGetID(param, out var studentID)) return null;
                    students = students.Where(student => student.Id == studentID);
                    break;
                case enStudentParamType.PersonID:
                    if (!TryGetID(param, out var personID)) return null;
                    students = students.Where(student => student.PersonID == personID);
                    break;
                case enStudentParamType.NationalNumber:
                    students = students.Where(student => student.Person.NationalNumber == param);
                    break;
                default:
                    return null;
            }

            return await students.FirstOrDefaultAsync();
        }

        public async Task<List<StudentInfo>> GetList()
        {
            return await ProjectActiveStudents().ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enStudentParamType paramType)
        {
            return paramType switch
            {
                enStudentParamType.ID => TryGetID(param, out var studentID) &&
                    await _dbSet.AnyAsync(student => student.Id == studentID),
                enStudentParamType.PersonID => TryGetID(param, out var personID) &&
                    await _dbSet.AnyAsync(student => student.PersonID == personID),
                enStudentParamType.NationalNumber =>
                    await _dbSet.AnyAsync(student => student.Person.NationalNumber == param),
                _ => false,
            };
        }

        public Task<bool> IsStudent(string param, enStudentParamType paramType) =>
            IsExistBy(param, paramType);

        public Task<bool> Update(clsStudent entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(true);
        }

        private static bool TryGetID(string value, out Guid id) =>
            Guid.TryParseExact(value, "D", out id) && id != Guid.Empty;
    }
}
