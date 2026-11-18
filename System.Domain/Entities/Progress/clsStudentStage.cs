using System;
using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Enrollment;
using System.Domain.Entities.Progress;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsStudentStage : clsBaseClass
    {
        public enStageStateType State { get; private set; }
        public Guid StudentAcademicEnrollmentID { get; private set; }
        public Guid StageID { get; private set; }
        public DateOnly Date {  get; private set; }
        public clsStudentAcademicEnrollment StudentAcademicEnrollment { get; private set; }
        public clsStage Stage{ get; private set; }

        protected clsStudentStage() { }

        public clsStudentStage(enStageStateType state, Guid studentAcademicEnrollmentID, Guid stageID, DateOnly date)
        {
            SetState(state);
            SetStudentAcademicEnrollmentID(studentAcademicEnrollmentID);
            SetStageID(stageID);
            SetDate(date);
            BaseSetCreated("System"); // Default, will be updated by Service
        }

        public void SetState(enStageStateType stateType)
        {
            State = stateType;
        }
        public void SetStudentAcademicEnrollmentID(Guid studentAcademicEnrollmentID)
        {
            if (studentAcademicEnrollmentID == Guid.Empty)
                throw new DomainException(DomainExMsg.StudentStage_StudentAcademicEnrollmentIDRequired);
            StudentAcademicEnrollmentID = studentAcademicEnrollmentID;
        }
        public void SetStageID(Guid stageID)
        {
            if (stageID == Guid.Empty)
            {
                throw DomainExMsg.StudentStage_StudentIDRequired;
            }
            StageID = stageID;
        }
        public void SetDate(DateOnly date )
        {
            if(date==default)
                Date = DateOnly.FromDateTime(DateTime.Now);
            else
            {
                Date = date;
            }
        }
    }
}
