using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsClass : clsBaseClass
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        public string Note { get; private set; }
        public Guid ProgramID { get; private set; } // Foreign Key to future Program entity
        public virtual clsProgram Program { get; private set; }

        protected clsClass()
        {

        }
        public clsClass(string name, int level,string note, Guid programID)
        {
            SetName(name);
            SetLevel(level);
            SetNote(note);
            SetProgramID(programID);
            
            BaseSetCreated("System");
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException(DomainExMsg.Class_NameRequired);
            }
            Name = name;
        }

        public void SetLevel(int level)
        {
            if (level <= 0)
            {
                throw new DomainException(DomainExMsg.Class_LevelRequired); // Using same exception name but logic differs
            }
            Level = level;
        }
        public void SetNote(string note)
        {
            if (string.IsNullOrWhiteSpace(note))
            {
                throw new DomainException(DomainExMsg.Class_NoteRequired);
            }
            Note = note;
        }
        public void SetProgramID(Guid programID)
        {
            if (programID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.Class_ProgramIDRequired);
            }
            ProgramID = programID;
        }
    }
}
