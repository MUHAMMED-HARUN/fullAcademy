using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsTeacher : clsBaseClass
    {
        public Guid PersonID { get; private set; }
        public clsPerson Person { get; private set; }

        protected clsTeacher() { } // For EF

        public clsTeacher(Guid personID)
        {
            if (personID == Guid.Empty)
                throw new DomainException(DomainExMsg.Teacher_PersonIDRequired);

            PersonID = personID;
            BaseSetCreated("System");
        }

        public void SetPersonID(Guid personID)
        {
            if (personID == Guid.Empty)
                throw new DomainException(DomainExMsg.Teacher_PersonIDRequired);
            PersonID = personID;
        }
    }
}
