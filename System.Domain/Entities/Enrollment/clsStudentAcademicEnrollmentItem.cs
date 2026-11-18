using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Enrollment
{
    public class clsStudentAcademicEnrollmentItem : clsBaseClass
    {
        public Guid StudentAcademicEnrollmentID { get; private set; }
        public Guid StageInfoID { get; private set; }
        public double From { get; private set; }
        public double To { get; private set; }
        public Guid StageID { get; private set; }
        public string StageName { get; private set; } = string.Empty;
        public Guid MatterID { get; private set; }
        public string MatterName { get; private set; } = string.Empty;
        public enUniteType UniteType { get; private set; }
        public string UniteTypeName { get; private set; } = string.Empty;
        public enProgressType ProgressType { get; private set; }
        public string ProgressTypeName { get; private set; } = string.Empty;

        public clsStudentAcademicEnrollment StudentAcademicEnrollment { get; private set; } = null!;

        protected clsStudentAcademicEnrollmentItem() { }

        public clsStudentAcademicEnrollmentItem(
            Guid stageInfoID,
            double from,
            double to,
            Guid stageID,
            string stageName,
            Guid matterID,
            string matterName,
            enUniteType uniteType,
            enProgressType progressType,
            string? uniteTypeName = null,
            string? progressTypeName = null)
        {
            if (stageInfoID == Guid.Empty || stageID == Guid.Empty || matterID == Guid.Empty)
                throw new DomainException(DomainExMsg.StudentAcademicEnrollment_SnapshotInvalid);
            if (from < 0 || to < from)
                throw new DomainException(DomainExMsg.StudentAcademicEnrollment_SnapshotInvalid);

            StageInfoID = stageInfoID;
            From = from;
            To = to;
            StageID = stageID;
            StageName = stageName ?? string.Empty;
            MatterID = matterID;
            MatterName = matterName ?? string.Empty;
            UniteType = uniteType;
            UniteTypeName = uniteTypeName ?? uniteType.ToString();
            ProgressType = progressType;
            ProgressTypeName = progressTypeName ?? progressType.ToString();
            BaseSetCreated("System");
        }

        internal void AttachTo(clsStudentAcademicEnrollment enrollment)
        {
            StudentAcademicEnrollment = enrollment;
            StudentAcademicEnrollmentID = enrollment.Id;
        }
    }
}
