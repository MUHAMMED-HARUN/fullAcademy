using System.Application.Common.Models;
using System.Application.Services.Progress.StageInfo.DTO;

namespace System.Application.Services.Progress.StageInfo.Query
{
    public sealed class GetAllStageInfoFilterQuery : BaseFilterQuery<StageInfoInfo>
    {
        public string? StageID { get; set; }

        public GetAllStageInfoFilterQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
