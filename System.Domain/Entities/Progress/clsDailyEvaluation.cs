using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Enrollment;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsDailyEvaluation : clsBaseClass
    {
        public Guid StudentAcademicEnrollmentID { get; private set; }
        public double From { get; private set; }
        public double To { get; private set; }
        public DateTime Date { get; private set; }
        public Guid StageInfoID{ get; private set; }
        public enLevelType LevelType { get; private set; }
        public clsStudentAcademicEnrollment StudentAcademicEnrollment { get; private set; }
        public clsStageInfo StageInfo{ get; private set; }

        // EF Core Constructor
        protected clsDailyEvaluation() : base()
        {
        }

        public clsDailyEvaluation(
            Guid studentAcademicEnrollmentID,
            DateTime date,
            double from,
            double  to,
            Guid stageInfoID,
            enLevelType levelType
          ) : base()
        {
            SetStudentAcademicEnrollmentID(studentAcademicEnrollmentID);
            SetDate(date);
            SetFrom(from);
            SetTo(to);
            SetStageInfoID(stageInfoID);
            SetLevelType(levelType);

            SetCreated("system");
        }

        public void SetStudentAcademicEnrollmentID(Guid studentAcademicEnrollmentID)
        {
            if (studentAcademicEnrollmentID == Guid.Empty)
                throw new DomainException(DomainExMsg.DailyEvaluation_StudentAcademicEnrollmentIDRequired);
            StudentAcademicEnrollmentID = studentAcademicEnrollmentID;
        }

        public void SetDate(DateTime date)
        {
            if (date == default)
                throw new DomainException(DomainExMsg.DailyEvaluation_DateRequired);
            Date = date;
        }

        public void SetFrom(double from)
        {
            if (from < 0)
                throw new DomainException(DomainExMsg.DailyEvaluation_FromInvalid);
            From = from;
        }

        public void SetTo(double to)
        {
            if (to < 0)
                throw new DomainException(DomainExMsg.DailyEvaluation_ToInvalid);
            if (to < From)
                throw new DomainException(DomainExMsg.DailyEvaluation_ToInvalid);
            To = to;
        }
        public void SetStageInfoID(Guid stageInfoID)
        {
            if (stageInfoID == Guid.Empty)
                throw new DomainException(DomainExMsg.DailyEvaluation_StageInfoIDRequired);
            StageInfoID= stageInfoID;
        }

        public void SetLevelType(enLevelType levelType)
        {
            LevelType = levelType;
        }

   
    }
}
