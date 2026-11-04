using Gridify;
using System.Application.Services.Person.Student.DTO;
using System.Application.Services.Person.Student.Query;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Person
{
    public interface IStudentRepository
    {
        Task<string> Add(clsStudent entity);// return ID as String
        Task<bool> Update(clsStudent entity);
        Task<List<StudentInfo>> GetList();
        Task<Paging<StudentInfo>> GetAllStudentFilter(GetAllStudentQuery requset);
        Task<bool> IsStudent(string param, enStudentParamType paramType);
        Task<StudentInfo?> GetBy(string param, enStudentParamType paramType);
        Task<clsStudent?> GetEntityBy(string param, enStudentParamType paramType);
        Task<bool> IsExistBy(string param, enStudentParamType paramType);
    }
}
