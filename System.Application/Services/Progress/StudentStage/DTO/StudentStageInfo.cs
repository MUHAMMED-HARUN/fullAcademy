
namespace System.Application.Services.Progress.StudentStage.DTO
{
    public class StudentStageInfo
    {
        public string StudentStageID { get; set; } = string.Empty;
        public string StageID { get; set; } = string.Empty;
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public string StageName { get; set; } = string.Empty;
        public string StudentStageState { get; set; } = string.Empty;
        public int StudentStageStateValue { get; set; }
        public DateOnly StudentStageDate { get; set; }

    }
}
