using System.Domain.Entities.BaseClass;
using System.Domain.Entities;
using System.Domain.Entities.AcademicPlan;
using System.Domain.Entities.Person;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Enrollment;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Enrollment
{
    public class clsStudentAcademicEnrollment : clsBaseClass
    {
        public Guid StudentID { get; private set; }
        public Guid AcademicPlanID { get; private set; }
        public int AcademicPlanVersion { get; private set; }
        public DateOnly EnrollmentDate { get; private set; }
        public enStudentAcademicEnrollmentStatus Status { get; private set; }
        public DateOnly ValidFrom { get; private set; }
        public DateOnly? ValidTo { get; private set; }

        public clsStudent Student { get; private set; } = null!;
        public clsAcademicPlan AcademicPlan { get; private set; } = null!;
        public ICollection<clsStudentAcademicEnrollmentItem> Items { get; private set; } = new HashSet<clsStudentAcademicEnrollmentItem>();
        public ICollection<clsStudentEnrollment> GroupEnrollments { get; private set; } = new HashSet<clsStudentEnrollment>();
        public ICollection<clsStudentStage> StudentStages { get; private set; } = new HashSet<clsStudentStage>();
        public ICollection<clsDailyEvaluation> DailyEvaluations { get; private set; } = new HashSet<clsDailyEvaluation>();

        protected clsStudentAcademicEnrollment() { }

        public clsStudentAcademicEnrollment(
            Guid studentID,
            Guid academicPlanID,
            int academicPlanVersion,
            DateOnly enrollmentDate,
            DateOnly validFrom,
            DateOnly? validTo,
            IEnumerable<clsStudentAcademicEnrollmentItem> items)
        {
            if (studentID == Guid.Empty || academicPlanID == Guid.Empty)
                throw new DomainException(DomainExMsg.StudentAcademicEnrollment_ReferenceRequired);
            if (academicPlanVersion <= 0 || enrollmentDate == default || validFrom == default ||
                (validTo.HasValue && validTo.Value < validFrom) || enrollmentDate < validFrom ||
                (validTo.HasValue && enrollmentDate > validTo.Value))
                throw new DomainException(DomainExMsg.StudentAcademicEnrollment_ValidityInvalid);

            var snapshots = items?.ToList() ?? new List<clsStudentAcademicEnrollmentItem>();
            if (snapshots.Count == 0)
                throw new DomainException(DomainExMsg.StudentAcademicEnrollment_ItemsRequired);

            StudentID = studentID;
            AcademicPlanID = academicPlanID;
            AcademicPlanVersion = academicPlanVersion;
            EnrollmentDate = enrollmentDate;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Status = enStudentAcademicEnrollmentStatus.Active;
            BaseSetCreated("System");

            foreach (var item in snapshots)
            {
                item.AttachTo(this);
                Items.Add(item);
            }
        }

        public bool IsValidOn(DateOnly date) =>
            Status == enStudentAcademicEnrollmentStatus.Active &&
            date >= ValidFrom && (!ValidTo.HasValue || date <= ValidTo.Value);

        public void Complete(string userId)
        {
            if (Status != enStudentAcademicEnrollmentStatus.Active)
                throw new DomainException(DomainExMsg.StudentAcademicEnrollment_NotActive);
            Status = enStudentAcademicEnrollmentStatus.Completed;
            SetModified(userId);
        }
    }
}
