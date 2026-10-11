using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Guardians
{
    public class clsGuardian : clsBaseClass
    {
        public Guid PersonID { get; private set; }
        public string? Occupation { get; private set; }
        public string? WorkPhone { get; private set; }

        public virtual clsPerson Person { get; private set; }

        protected clsGuardian() { }

        public clsGuardian(Guid personId, string? occupation = null, string? workPhone = null)
        {
            SetPersonID(personId);
            Occupation = occupation;
            WorkPhone = workPhone;

            BaseSetCreated("System");
        }

        public void SetPersonID(Guid personId)
        {
            if (personId == Guid.Empty)
                throw new DomainException(DomainExMsg.Guardian_PersonIDRequired);

            PersonID = personId;
        }

        public void SetOccupation(string? occupation) { Occupation = occupation; }
        public void SetWorkPhone(string? workPhone) { WorkPhone = workPhone; }
    }
}