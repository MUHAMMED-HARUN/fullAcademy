using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsStage : clsBaseClass
    {
        public string Name { get; private set; }
        public string? Notes { get; private set; }
        public Guid ProgramID { get; private set; }

        public Guid? PrevStageID { get; private set; }
        public clsProgram Program{ get; private set; }

        public clsStage PrevStage { get; private set; }
        public ICollection<clsStudentStage> StudentStages = new HashSet<clsStudentStage>();
        public ICollection<clsStageInfo> StageInfoItems = new HashSet<clsStageInfo>();

        protected clsStage() { }

        public clsStage(string name, string? notes,Guid programID,Guid prevStageID)
        {
            SetName(name);
            SetNotes(notes);
            SetProgramID(programID);
            SetPrevStageID(prevStageID);
            BaseSetCreated("System"); // Default, will be updated by Service
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException(DomainExMsg.Stage_NameRequired);
            }
            Name = name;
        }
        public void SetProgramID(Guid programID)
        {
            if (Guid.Empty==programID)
            {
                throw new DomainException(DomainExMsg.Stage_ProgramIDRequired);
            }
            ProgramID  = programID;
        }
        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
        public void SetPrevStageID(Guid stageID)
        {
            if (Guid.Empty == stageID)
            {
                PrevStageID = null;
                return;
            }
            PrevStageID = stageID;
        }
    }
}
