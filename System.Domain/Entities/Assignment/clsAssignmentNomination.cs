using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsAssignmentNomination : clsBaseClass
    {
        public Guid StudentEnrollmentID { get; private set; }
        public Guid AssignmentScopeID { get; private set; }
        public DateOnly Date { get; private set; }
        public Guid NominatedByID { get; private set; }
        public enUserType NominatedByType { get; private set; }
        public enAssignmentType NominationState { get; private set; }
  

        public clsStudentEnrollment StudentEnrollment { get; private set; }
        public clsAssignmentScope AssignmentScope{ get; private set; }
        protected clsAssignmentNomination() { }

        public clsAssignmentNomination(Guid studentEnrollmentID, Guid assignmentScopeID, DateOnly date, Guid nominatedByID, enUserType nominatedByType, enAssignmentType nominationState)
        {
            SetStudentEnrollmentID(studentEnrollmentID);
            SetAssignmentScopeID(assignmentScopeID);
            SetDate(date);
            SetNominatedByID(nominatedByID);
            SetNominatedByType(nominatedByType);
            SetNominationState(nominationState);
            BaseSetCreated("System");
        }

        public void SetStudentEnrollmentID(Guid studentEnrollmentID)
        {
            if (studentEnrollmentID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.AssignmentNomination_StudentEnrollmentIDRequired);
            }
            StudentEnrollmentID = studentEnrollmentID;
        }

        public void SetAssignmentScopeID(Guid assignmentScopeID)
        {
            if (assignmentScopeID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.AssignmentNomination_AssignmentScopeIDRequired);
            }
            AssignmentScopeID = assignmentScopeID;
        }

        public void SetDate(DateOnly date)
        {
            Date = date;
        }

        public void SetNominatedByID(Guid nominatedByID)
        {
            if (nominatedByID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.AssignmentNomination_NominatedByIDRequired);
            }
            NominatedByID = nominatedByID;
        }

        public void SetNominatedByType(enUserType nominatedByType)
        {
            NominatedByType = nominatedByType;
        }

        public void SetNominationState(enAssignmentType nominationState)
        {
            NominationState = nominationState;
        }
    }
}