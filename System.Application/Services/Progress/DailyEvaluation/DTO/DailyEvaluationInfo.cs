
namespace System.Application.Services.Progress.DailyEvaluation.DTO
{
    public class DailyEvaluationInfo
    {
        public string DailyEvaluationID { get; set; }
        public string StudentEnrollmentID { get; set; }
        public string StudentName { get; set; }
        public string GroupName { get; set; }
        public string GroupID{ get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public DateTime Date { get; set; }
        public string StageInfoID{ get; set; }
        public string LevelType { get; set; }

    }
}
