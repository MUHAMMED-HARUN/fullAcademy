using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsResult : clsBaseClass
    {
        public Guid AssignmentID { get; private set; }
        public Guid AssignmentScopeInfoID { get; private set; }
        public double Score { get; private set; }
        public string? Notes { get; private set; }
        public clsAssignment Assignment { get; private set; }

        public clsAssignmentScopeinfo AssignmentScopeInfo { get; private set; }
      

        protected clsResult() { }

        public clsResult(Guid assignmentID, Guid assignmentScopeInfoID, double score, string? notes)
        {
            SetAssignmentID(assignmentID);
            SetAssignmentScopeInfoID(assignmentScopeInfoID);
            SetScore(score);
            SetNotes(notes);
       
            BaseSetCreated("System"); // Default, will be updated by Service
            AddDomainEvent(new ResultCreatedEvent(Id, assignmentID, assignmentScopeInfoID,score));
        }

        public void SetAssignmentID(Guid assignmentID)
        {
            if (assignmentID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.Result_AssignmentIDRequired);
            }
            AssignmentID = assignmentID;
        }

        public void SetAssignmentScopeInfoID(Guid assignmentScopeInfoID)
        {
            if (assignmentScopeInfoID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.Result_AssignmentScopeInfoIDRequired);
            }
            AssignmentScopeInfoID = assignmentScopeInfoID;
        }

        public void SetScore(double score)
        {
            if (score < 0)
            {
                throw new DomainException(DomainExMsg.Result_ScoreMustBeNonNegative);
            }
            Score = score;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
      
    }
}
