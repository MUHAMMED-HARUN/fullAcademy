using Gridify;
using System.Application.Abstractions;
using System.Application.Common.Models;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Application.Services.Person.Student.DTO;

namespace System.Application.Services.Enrollment.StudentEnrollment.Query
{
    public class GetAllStudentEnrollmentQuery : BaseFilterQuery<StudentEnrollmentInfoView>, IQuery
    {
        public GetAllStudentEnrollmentQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
