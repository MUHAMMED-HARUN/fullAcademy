

using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.StageInfo.DTO
{
    public class StageInfoInfo
    {
        public string StageInfoID {  get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public string StageID { get; set; }
        public string StageName { get; set; }
        public string MatterID { get; set; }
        public string MatterName { get; set; }
        public enUniteType UniteType { get; set; }
        public string UniteTypeName { get; set; }
        public enProgressType ProgressType { get; set; }
        public string ProgressTypeName { get; set; }

    }
}
