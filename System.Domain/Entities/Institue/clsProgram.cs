
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsProgram : clsBaseClass
    {
        public string Name { get; private set; }
        public string? Notes { get; private set; }

        public clsProgram(string name, string? notes)
        {
            SetName(name);
            SetNotes(notes);
            
            BaseSetCreated("System");
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException(DomainExMsg.Program_NameRequired);
            }
            Name = name;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
    }
}
