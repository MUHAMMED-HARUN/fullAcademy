using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsAssignment : clsBaseClass
    {
        public Guid AssignmentNominationID { get; private set; }
        public Guid CommitteeID { get; private set; }
        public enCommitteeType CommitteeType { get; private set; }
        public DateOnly ScheduledDate { get; private set; }
        public enAssignmentType AssignmentState { get; private set; }
        public bool? IsPassed { get; private set; }
        public double? TotalScore { get; private set; }


        public clsAssignmentNomination AssignmentNomination { get; private set; }
        protected clsAssignment() { }

        public clsAssignment(Guid assignmentNominationID, Guid committeeID, enCommitteeType committeeType, DateOnly scheduledDate, enAssignmentType assignmentState, bool? isPassed, double? totalScore)
        {
            SetAssignmentNominationID(assignmentNominationID);
            SetCommitteeID(committeeID);
            SetCommitteeType(committeeType);
            SetScheduledDate(scheduledDate);
            SetAssignmentState(assignmentState);
            SetIsPassed(isPassed);
            SetTotalScore(totalScore);
            BaseSetCreated("System");
            AddDomainEvent(new AssigmentCreatedEvent(Id, assignmentNominationID));

        }


        public void SetAssignmentNominationID(Guid assignmentNominationID)
        {
            if (assignmentNominationID == Guid.Empty)
                throw new DomainException(DomainExMsg.Assignment_AssignmentNominationIDRequired);
            AssignmentNominationID = assignmentNominationID;
        }

        public void SetCommitteeID(Guid committeeID)
        {
            if (committeeID == Guid.Empty)
                throw new DomainException(DomainExMsg.Assignment_CommitteeIDRequired);
            CommitteeID = committeeID;
        }

        public void SetCommitteeType(enCommitteeType committeeType)
        {
            CommitteeType = committeeType;
        }

        public void SetScheduledDate(DateOnly scheduledDate)
        {
            ScheduledDate = scheduledDate;
        }

        public void SetAssignmentState(enAssignmentType assignmentState)
        {
            AssignmentState = assignmentState;
        }

        public void SetIsPassed(bool? isPassed)
        {
            IsPassed = isPassed;
        }
        public void SetTotalScore(double? totalScore)
        {
            TotalScore = totalScore;
        }
        public override void SetDeleted(string userId)
        {
            base.SetDeleted(userId);
            AddDomainEvent(new AssigmentDeletedEvent(Id,AssignmentNominationID,AssignmentState));
        }
    }
}