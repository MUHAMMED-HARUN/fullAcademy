using System.Application.Abstractions;
using System.Application.Common.Models;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class GetTeacherGroupsQuery : BaseFilterQuery<TeacherGroupInfo>, IQuery
    {
        public GetTeacherGroupsQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
