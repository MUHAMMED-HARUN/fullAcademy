using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO
{
    public sealed class StudentAcademicEnrollmentInfo
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicPlanID { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public int AcademicPlanVersion { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public enStudentAcademicEnrollmentStatus Status { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly? ValidTo { get; set; }
        public List<StudentAcademicEnrollmentItemInfo> Items { get; set; } = new();
    }

    public sealed class StudentAcademicEnrollmentLookupInfo
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicPlanID { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public int AcademicPlanVersion { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public enStudentAcademicEnrollmentStatus Status { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly? ValidTo { get; set; }
        public int ItemCount { get; set; }
    }

    public sealed class StudentAcademicEnrollmentItemInfo
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

    public sealed class AcademicPlanEnrollmentSource
    {
        public string AcademicPlanID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Version { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
        public List<StudentAcademicEnrollmentItemInfo> Items { get; set; } = new();
    }

    public sealed class StudentAcademicEnrollmentMembershipInfo
    {
        public string StudentAcademicEnrollmentID { get; set; } = string.Empty;
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string AcademicPlanName { get; set; } = string.Empty;
        public int AcademicPlanVersion { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly? ValidTo { get; set; }
    }
}
