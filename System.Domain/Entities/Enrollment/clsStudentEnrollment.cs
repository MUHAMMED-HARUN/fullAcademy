using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsStudentEnrollment : clsBaseClass
    {
        public Guid StudentID { get; private set; }
        public Guid GroupID { get; private set; }
        public DateOnly Date { get; private set; }
        
        public clsStudent Student { get; private set; }
        public clsGroup Group { get; private set; }
        public ICollection<clsStudentStage> StudentStages = new HashSet<clsStudentStage>();

        public clsStudentEnrollment(Guid studentID, Guid groupID, DateOnly date)
        {
            SetStudentID(studentID);
            SetGroupID(groupID);
            SetDate(date);
            SetID(Guid.NewGuid());
            
            BaseSetCreated("System");
        }

        public void SetStudentID(Guid studentID)
        {
            if (studentID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.StudentEnrollment_StudentIDRequired);
            }
            StudentID = studentID;
        }

        public void SetGroupID(Guid groupID)
        {
            if (groupID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.StudentEnrollment_GroupIDRequired);
            }
            GroupID = groupID;
        }

        public void SetDate(DateOnly date)
        {
             if (date == default)
            {
                throw new DomainException(DomainExMsg.StudentEnrollment_DateRequired);
            }
            Date = date;
        }
    }
}
