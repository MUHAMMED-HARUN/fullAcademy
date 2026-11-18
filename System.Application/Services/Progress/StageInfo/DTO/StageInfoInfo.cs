

using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.StageInfo.DTO
{
    public class StageInfoInfo
    {
        public string StageInfoID { get; set; } = string.Empty;
        public double From { get; set; }
        public double To { get; set; }
        public string StageID { get; set; } = string.Empty;
        public string StageName { get; set; } = string.Empty;
        public string MatterID { get; set; } = string.Empty;
        public string MatterName { get; set; } = string.Empty;
        public enUniteType UniteType { get; set; }
        public string UniteTypeName { get; set; } = string.Empty;
        public enProgressType ProgressType { get; set; }
        public string ProgressTypeName { get; set; } = string.Empty;

    }
}
