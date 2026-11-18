using System.Domain.Entities.BaseClass;
using System.Domain.Entities.AcademicPlan;
using System.Domain.Enums.Finance;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Finance
{
    public class clsFeeStructure : clsBaseClass
    {
        public string Name { get; private set; }
        public enFeeType FeeType { get; private set; }
        public decimal Amount { get; private set; }
        public Guid AcademicPlanID { get; private set; }
        public string? Notes { get; private set; }

        // Navigation properties
        public clsAcademicPlan AcademicPlan { get; private set; }
        public ICollection<clsStudentInvoice> StudentInvoices { get; private set; } = new HashSet<clsStudentInvoice>();

        protected clsFeeStructure() { }

        public clsFeeStructure(
            string name,
            enFeeType feeType,
            decimal amount,
            Guid academicPlanID,
            string? notes)
        {
            SetName(name);
            SetFeeType(feeType);
            SetAmount(amount);
            SetAcademicPlanID(academicPlanID);
            SetNotes(notes);
            BaseSetCreated("System");
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(DomainExMsg.FeeStructure_NameRequired);
            Name = name;
        }

        public void SetFeeType(enFeeType feeType)
        {
            FeeType = feeType;
        }

        public void SetAmount(decimal amount)
        {
            if (amount < 0)
                throw new DomainException(DomainExMsg.FeeStructure_AmountInvalid);
            Amount = amount;
        }

        public void SetAcademicPlanID(Guid academicPlanID)
        {
            if (academicPlanID == Guid.Empty)
                throw new DomainException(DomainExMsg.FeeStructure_AcademicPlanRequired);
            AcademicPlanID = academicPlanID;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
    }
}
