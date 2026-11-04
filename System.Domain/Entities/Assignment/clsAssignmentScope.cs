using System.Domain.Entities.BaseClass;
using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Progress;
using System.Domain.Entities.Progress;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsAssignmentScope : clsBaseClass
    {
        public string Name { get; private set; }
        public string? Notes { get; private set; }
        public Guid StageID { get; private set; }
        public Guid? PrevAssignmentScopeID { get; private set; }
        public clsStage Stage { get; private set; }

        public clsAssignmentScope PrevAssignmentScope { get; private set; }
      

        protected clsAssignmentScope() { }

        public clsAssignmentScope(string name, string? notes, Guid stageID, Guid? prevAssignmentScopeID)
        {
            SetName(name);
            SetNotes(notes);
            SetStageID(stageID);
            SetPrevAssignmentScopeID(prevAssignmentScopeID);
            BaseSetCreated("System"); // Default, will be updated by Service
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException(DomainExMsg.AssignmentScope_NameRequired);
            }
            Name = name;
        }

        public void SetStageID(Guid stageID)
        {
            if (stageID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.AssignmentScope_StageIDRequired);
            }
            StageID = stageID;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }

        public void SetPrevAssignmentScopeID(Guid? prevAssignmentScopeID)
        {
            if (prevAssignmentScopeID == Guid.Empty)
            {
                PrevAssignmentScopeID = null;
                return;
            }
            PrevAssignmentScopeID = prevAssignmentScopeID;
        }
    }
}
