using System.Domain.Entities.BaseClass;
using System.Domain.Entities;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Progress
{
    public class clsStage : clsBaseClass
    {
        public string Name { get; private set; }
        public string? Notes { get; private set; }
        public Guid ClassID { get; private set; }

        public Guid? PrevStageID { get; private set; }
        public clsClass Class { get; private set; }

        public clsStage? PrevStage { get; private set; }
        public ICollection<clsStage> NextStages { get; private set; } = new HashSet<clsStage>();
        public ICollection<clsStudentStage> StudentStages = new HashSet<clsStudentStage>();
        public ICollection<clsStageInfo> StageInfoItems = new HashSet<clsStageInfo>();

        protected clsStage() { }

        public clsStage(string name, string? notes, Guid classID, Guid? prevStageID)
        {
            SetName(name);
            SetNotes(notes);
            SetClassID(classID);
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
        public void SetClassID(Guid classID)
        {
            if (Guid.Empty == classID)
            {
                throw new DomainException(DomainExMsg.Stage_ClassIDRequired);
            }
            ClassID = classID;
        }
        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
        public void SetPrevStageID(Guid? stageID)
        {
            PrevStageID = stageID == Guid.Empty ? null : stageID;
        }
    }
}
