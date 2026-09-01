using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.StudentEnrollment.Query
{
    public class GetStudentEnrollmentByQuery : IRequest<TResult<StudentEnrollmentInfo>>, IQuery
    {
        public string Param { get; set; }
        public enStudentEnrollmentType ParamType { get; set; }
    }
}
