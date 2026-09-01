
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsTeachingAssignment : clsBaseClass
    {
        public Guid TeacherID { get; private set; }
        public Guid GroupID { get; private set; }
        public Guid SubjectID { get; private set; }
        public DateOnly Date { get; private set; }

        public clsTeacher Teacher { get; private set; }
        public clsGroup Group { get; private set; }
        public clsSession Subject { get; private set; }

        public clsTeachingAssignment(Guid teacherID, Guid groupID, Guid subjectID,DateOnly date)
        {
            SetTeacherID(teacherID);
            SetGroupID(groupID);
            SetSubjectID(subjectID);
            SetDate(date);
            BaseSetCreated("System");
        }

        public void SetTeacherID(Guid teacherID)
        {
            if (teacherID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.TeachingAssignment_TeacherIDRequired);
            }
            TeacherID = teacherID;
        }

        public void SetGroupID(Guid groupID)
        {
            if (groupID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.TeachingAssignment_GroupIDRequired);
            }
            GroupID = groupID;
        }

        public void SetSubjectID(Guid subjectID)
        {
             if (subjectID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.TeachingAssignment_SubjectIDRequired);
            }
            SubjectID = subjectID;
        }
        public void SetDate(DateOnly date)
        {
            if (date == default)
            {
              Date = DateOnly.FromDateTime(DateTime.Now);
            }
            Date= date;
        }
    }
}
