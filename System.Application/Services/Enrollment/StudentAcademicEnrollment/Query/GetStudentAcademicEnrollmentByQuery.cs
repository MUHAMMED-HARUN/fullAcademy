using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Query
{
    public sealed class GetStudentAcademicEnrollmentByQuery : IRequest<TResult<StudentAcademicEnrollmentInfo>>, IQuery
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
    }
}
