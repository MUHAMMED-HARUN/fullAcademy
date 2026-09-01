using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class GetTeachingAssignmentByQuery : IRequest<TResult<TeachingAssignmentInfo>>, IQuery
    {
        public string Param { get; set; }
        public enTeachingAssignmentType ParamType { get; set; }
    }
}
