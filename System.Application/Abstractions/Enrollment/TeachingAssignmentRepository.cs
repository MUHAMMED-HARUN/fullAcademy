using Gridify;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Application.Services.Enrollment.TeachingAssignment.Query;
using System.Application.Services.Person.Student.DTO;
using System.Application.Services.Person.Student.Query;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Abstractions.Enrollment
{
    public interface ITeachingAssignmentRepository
    {
        Task<string> Add(clsTeachingAssignment entity);// return ID as String
        Task<bool> Update(clsTeachingAssignment entity);
        Task<List<TeachingAssignmentInfo>> GetList();
        Task<Paging<TeachingAssignmentInfo>> GetListFilter(GetAllTeachingAssignmentQuery requset);
        Task<Paging<TeacherGroupInfo>> GetTeacherGroups(GetTeacherGroupsQuery requset, string TeacherID);
        Task<TeachingAssignmentInfo> GetBy(string param, enTeachingAssignmentType paramType);
        Task<clsTeachingAssignment> GetEntityBy(string param, enTeachingAssignmentType paramType);
        Task<bool> IsExistBy(string param, enTeachingAssignmentType paramType);
        Task<bool> IsTeacherEnrolled(string teachetID, string groupID,string subjectID);
    }
}
