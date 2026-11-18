using MediatR;
using System.Application.Common;
using System.Application.Common.Models;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Application.Abstractions;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class GetAllTeachingAssignmentQuery : BaseFilterQuery<TeachingAssignmentInfo>, IQuery
    {
        public GetAllTeachingAssignmentQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
