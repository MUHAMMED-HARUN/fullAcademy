using System.Application.Common.Models;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    public sealed class GetMyTeachingGroupsQuery : BaseFilterQuery<GroupInfo>
    {
        public GetMyTeachingGroupsQuery()
        {
            Page = 1;
            PageSize = 100;
        }
    }
}
