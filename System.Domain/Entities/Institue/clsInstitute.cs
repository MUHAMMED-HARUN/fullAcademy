using System.Domain.Entities.Address;
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Institue
{
    public class clsInstitute : clsBaseClass
    {
        public string Name { get; private set; }
        public Guid AddressID { get; private set; }
        public string? Notes { get; private set; }
        public clsAddress Address { get; private set; }

        protected clsInstitute()
        {
            
        }
        public clsInstitute(string name, Guid addressID, string? notes)
        {
            SetName(name);
            SetAddressID(addressID);
            SetNotes(notes);
            
            BaseSetCreated("System");
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException(DomainExMsg.Institute_NameRequired);
            }
            Name = name;
        }

        public void SetAddressID(Guid addressID)
        {
            if (addressID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.Institute_AddressIDRequired);
            }
            AddressID = addressID;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
    }
}
