using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.StudentEnrollment.Query
{
    public class IsExistStudentEnrollmentByQuery : IRequest<TResult<bool>>, IQuery
    {
        public string Param { get; set; }
        public enStudentEnrollmentType ParamType { get; set; }
    }
}
