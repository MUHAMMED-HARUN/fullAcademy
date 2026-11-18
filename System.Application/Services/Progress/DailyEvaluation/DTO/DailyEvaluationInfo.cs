
namespace System.Application.Services.Progress.DailyEvaluation.DTO
{
    public class DailyEvaluationInfo
    {
        public string DailyEvaluationID { get; set; } = string.Empty;
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public double From { get; set; }
        public double To { get; set; }
        public DateTime Date { get; set; }
        public string StageInfoID { get; set; } = string.Empty;
        public string StageInfoName { get; set; } = string.Empty;
        public string LevelType { get; set; } = string.Empty;

    }
}
