using System;
using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Guardians;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Guardians
{
    public class clsStudentGuardian : clsBaseClass
    {
        public Guid StudentID { get; private set; }
        public Guid GuardianID { get; private set; }
        public RelationshipType Relationship { get; private set; }
        public bool IsPrimary { get; private set; }
        public bool CanPickUp { get; private set; }

        public virtual clsGuardian Guardian { get; private set; }

        protected clsStudentGuardian() { }

        public clsStudentGuardian(
            Guid studentId,
            Guid guardianId,
            RelationshipType relationship,
            bool isPrimary = false,
            bool canPickUp = true)
        {
            SetStudentID(studentId);
            SetGuardianID(guardianId);
            Relationship = relationship;
            IsPrimary = isPrimary;
            CanPickUp = canPickUp;

            BaseSetCreated("System");
        }

        public void SetStudentID(Guid studentId)
        {
            if (studentId == Guid.Empty)
                throw new DomainException(DomainExMsg.StudentGuardian_StudentIDRequired);

            StudentID = studentId;
        }

        public void SetGuardianID(Guid guardianId)
        {
            if (guardianId == Guid.Empty)
                throw new DomainException(DomainExMsg.StudentGuardian_GuardianIDRequired);

            GuardianID = guardianId;
        }

        public void SetRelationship(RelationshipType relationship) { Relationship = relationship; }
        public void SetIsPrimary(bool isPrimary) { IsPrimary = isPrimary; }
        public void SetCanPickUp(bool canPickUp) { CanPickUp = canPickUp; }
    }
}