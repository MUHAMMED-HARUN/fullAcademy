using System;
using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Institue;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsInstituteClass : clsBaseClass
    {
        public Guid InstituteID { get; private set; }
        public Guid ClassID { get; private set; }
        
        public clsInstitute Institute { get; private set; }
        public clsClass Class { get; private set; }

        public clsInstituteClass(Guid instituteID, Guid classID)
        {
            SetInstituteID(instituteID);
            SetClassID(classID);
            
            BaseSetCreated("System");
        }

        public void SetInstituteID(Guid instituteID)
        {
            if (instituteID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.InstituteClass_InstituteIDRequired);
            }
            InstituteID = instituteID;
        }

        public void SetClassID(Guid classID)
        {
             if (classID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.InstituteClass_ClassIDRequired);
            }
            ClassID = classID;
        }
    }
}
