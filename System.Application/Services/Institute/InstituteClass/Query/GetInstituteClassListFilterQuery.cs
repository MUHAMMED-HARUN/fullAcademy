using System.Application.Common.Models;
using System.Application.Services.Institute.Program.DTO;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    public sealed class GetInstituteClassListFilterQuery : BaseFilterQuery<InstituteClassInfo>
    {
        public GetInstituteClassListFilterQuery()
        {
            Page = 1;
            PageSize = 20;
        }
    }
}
