using System.Application.Common.Models;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Query
{
    public sealed class GetAllStudentAcademicEnrollmentFilterQuery : BaseFilterQuery<StudentAcademicEnrollmentLookupInfo>
    {
        public string? GroupID { get; set; }
        public bool ActiveOnly { get; set; }

        public GetAllStudentAcademicEnrollmentFilterQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
