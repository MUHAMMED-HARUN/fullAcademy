
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsGroup : clsBaseClass
    {
        public Guid InstituteClassID { get; private set; }
        public string GroupName { get; private set; }
        public string Code { get; private set; }

        public clsInstituteClass InstituteClass { get; private set; }

        protected clsGroup()
        {
            
        }
        public clsGroup(Guid instituteClassID, string groupName, string code)
        {
            SetInstituteClassID(instituteClassID);
            SetGroupName(groupName);
            SetCode(code);
            
            BaseSetCreated("System");
        }

        public void SetInstituteClassID(Guid instituteClassID)
        {
            if (instituteClassID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.Group_InstituteClassIDRequired);
            }
            InstituteClassID = instituteClassID;
        }

        public void SetGroupName(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new DomainException(DomainExMsg.Group_NameRequired);
            }
            GroupName = groupName;
        }

        public void SetCode(string code)
        {
             if (string.IsNullOrWhiteSpace(code))
            {
                throw new DomainException(DomainExMsg.Group_CodeRequired);
            }
            Code = code;
        }
    }
}
