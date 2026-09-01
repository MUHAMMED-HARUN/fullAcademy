using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class IsExistTeachingAssignmentByQuery : IRequest<TResult<bool>>, IQuery
    {
        public string Param { get; set; }
        public enTeachingAssignmentType ParamType { get; set; }
    }
}
