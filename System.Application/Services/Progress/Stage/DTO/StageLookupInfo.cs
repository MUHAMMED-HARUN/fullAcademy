namespace System.Application.Services.Progress.Stage.DTO
{
    public sealed class StageLookupInfo
    {
        public string StageID { get; set; } = string.Empty;
        public string StageName { get; set; } = string.Empty;
        public string? StageNote { get; set; }
        public string ClassID { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string? PrevStageID { get; set; }
        public string? PrevStageName { get; set; }
    }
}
