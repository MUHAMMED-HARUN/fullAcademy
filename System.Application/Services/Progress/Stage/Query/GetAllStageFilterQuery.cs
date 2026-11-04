using System.Application.Common.Models;
using System.Application.Services.Progress.Stage.DTO;

namespace System.Application.Services.Progress.Stage.Query
{
    public sealed class GetAllStageFilterQuery : BaseFilterQuery<StageLookupInfo>
    {
        public string? ClassID { get; set; }
        public string? ExcludeStageID { get; set; }

        public GetAllStageFilterQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
