using System;

namespace System.Application.Services.Enrollment.StudentEnrollment.DTO
{
    public class StudentEnrollmentInfoView
    {
        public Guid EnrollmentID { get; set; }
        public Guid StudentID { get; set; }
        public string StudentName { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public DateOnly Date { get; set; }
        public bool CanTrack { get; set; }
        public bool CanNominate { get; set; }
        public bool CanAddToStage { get; set; }

    }
    
}
