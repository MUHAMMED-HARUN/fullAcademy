using Gridify;
using System.Application.Services.Person.Teacher.DTO;
using System.Application.Services.Person.Teacher.Query;
using System.Domain.Entities;
using System.Domain.Enums.Person;


namespace System.Application.Abstractions.Person
{
    public interface ITeacherRepository
    {
        Task<string> Add(clsTeacher entity);// return ID as String
        Task<bool> Update(clsTeacher entity);
        Task<List<TeacherInfo>> GetList();
        Task<Paging<TeacherInfo>> GetAllTeacherFilter(GetAllTeacherQuery requset);
        Task<bool> IsTeacher(string param, enTeacherParamType paramType);
        Task<TeacherInfo?> GetBy(string param, enTeacherParamType paramType);
        Task<clsTeacher?> GetEntityBy(string param, enTeacherParamType paramType);
        Task<bool> IsExistBy(string param, enTeacherParamType paramType);
    }
}
