using System;
using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Progress;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsStudentStage : clsBaseClass
    {
        public enStageStateType State { get; private set; }
        public Guid EnrollmentStudentID { get; private set; }
        public Guid StageID { get; private set; }
        public DateOnly Date {  get; private set; }
        public clsStudentEnrollment StudentEnrollment{ get; private set; }
        public clsStage Stage{ get; private set; }

        protected clsStudentStage() { }

        public clsStudentStage(enStageStateType state, Guid studentID, Guid stageID, DateOnly date)
        {
            SetState(state);
            SetEnrollmentStudentID(studentID);
            SetStageID(stageID);
            SetDate(date);
            BaseSetCreated("System"); // Default, will be updated by Service
        }

        public void SetState(enStageStateType stateType)
        {
            State = stateType;
        }
        public void SetEnrollmentStudentID(Guid enrollmentstudntID)
        {
            if (enrollmentstudntID == Guid.Empty)
            {
                throw DomainExMsg.StudentStage_StudentIDRequired;
            }
EnrollmentStudentID = enrollmentstudntID;
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
