using Gridify;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Enrollment.StudentEnrollment.Query;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Application.Services.Enrollment.TeachingAssignment.Query;
using System.Application.Services.Person.Student.Query;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Abstractions.Enrollment
{
    public interface IStudentEnrollmentRepository
    {
        Task<string> Add(clsStudentEnrollment entity);// return ID as String
        Task<bool> Update(clsStudentEnrollment entity);
        Task<List<StudentEnrollmentInfo>> GetList();
        Task<Paging<StudentEnrollmentInfoView>> GetGroupStudentss(GetAllStudentEnrollmentQuery requset, string TeacherID);

        Task<StudentEnrollmentInfo> GetBy(string param, enStudentEnrollmentType paramType);
        Task<clsStudentEnrollment> GetEntityBy(string param, enStudentEnrollmentType paramType);
        Task<bool> IsExistBy(string param, enStudentEnrollmentType paramType);
        Task<bool> IsStudentEnrolled(string studentID, string groupID);
    }
}
