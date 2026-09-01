using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Person
{
    public class clsStudent : clsBaseClass
    {
        public Guid PersonID { get; private set; }
        public clsPerson Person { get; private set; }

        protected clsStudent() { } // For EF
        public clsStudent(Guid personID)
        {
            if (personID == Guid.Empty)
                throw new DomainException(DomainExMsg.Student_PersonIDRequired);

            PersonID = personID;
            BaseSetCreated("System");
        }

        public void SetPersonID(Guid personID)
        {
            if (personID == Guid.Empty)
                throw new DomainException(DomainExMsg.Student_PersonIDRequired);
            PersonID = personID;
        }

    }
}
