
namespace System.Application.Services.Enrollment.TeachingAssignment.DTO
{
    public class TeachingAssignmentInfo
    {
        
        public string EnrollmentID { get; set; }
        public string TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public DateOnly EnrollmentDate { get; set; }
    }
}
