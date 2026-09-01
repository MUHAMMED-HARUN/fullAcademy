using System;

namespace System.Application.Services.Enrollment.StudentEnrollment.DTO
{
    public class StudentEnrollmentInfo
    {
        public string EnrollmentID { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public DateOnly Date { get; set; }
    }
}
