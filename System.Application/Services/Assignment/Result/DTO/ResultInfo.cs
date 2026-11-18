namespace System.Application.Services.Assignment.Result.DTO
{
    public sealed class ResultInfo
    {
        public string ResultID { get; set; } = string.Empty;
        public string AssignmentID { get; set; } = string.Empty;
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public string AssignmentScopeName { get; set; } = string.Empty;
        public string AssignmentScopeInfoID { get; set; } = string.Empty;
        public string StageInfoMatter { get; set; } = string.Empty;
        public short MaxScore { get; set; }
        public short MinPassScore { get; set; }
        public double Score { get; set; }
        public string? Notes { get; set; }
    }
}
