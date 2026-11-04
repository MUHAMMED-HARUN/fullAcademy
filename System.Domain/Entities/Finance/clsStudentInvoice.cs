using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Finance;
using System.Domain.Entities.Person;
using System.Domain.Enums.Finance;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Finance
{
    public class clsStudentInvoice : clsBaseClass
    {
        public Guid StudentID { get; private set; }
        public Guid FeeStructureID { get; private set; }
        public DateOnly IssueDate { get; private set; }
        public DateOnly DueDate { get; private set; }
        public decimal OriginalAmount { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal RemainingAmount { get; private set; }
        public enInvoiceStatus Status { get; private set; }
        public string? Notes { get; private set; }

        // Navigation properties
        public clsStudent Student { get; private set; }
        public clsFeeStructure FeeStructure { get; private set; }
        public ICollection<clsPayment> Payments { get; private set; } = new HashSet<clsPayment>();

        protected clsStudentInvoice() { }

        public clsStudentInvoice(
            Guid studentID,
            Guid feeStructureID,
            DateOnly issueDate,
            DateOnly dueDate,
            decimal originalAmount,
            decimal discountAmount,
            string? notes)
        {
            SetStudentID(studentID);
            SetFeeStructureID(feeStructureID);
            SetIssueDate(issueDate);
            SetDueDate(dueDate);
            SetOriginalAmount(originalAmount);
            SetDiscountAmount(discountAmount);
            SetNotes(notes);
            BaseSetCreated("System");
        }

        public void SetStudentID(Guid studentID)
        {
            if (studentID == Guid.Empty)
                throw new DomainException(DomainExMsg.StudentInvoice_StudentIDRequired);
            StudentID = studentID;
        }

        public void SetFeeStructureID(Guid feeStructureID)
        {
            if (feeStructureID == Guid.Empty)
                throw new DomainException(DomainExMsg.StudentInvoice_FeeStructureIDRequired);
            FeeStructureID = feeStructureID;
        }

        public void SetIssueDate(DateOnly issueDate)
        {
            if (issueDate == default)
                IssueDate = DateOnly.FromDateTime(DateTime.Now);
            else
                IssueDate = issueDate;
        }

        public void SetDueDate(DateOnly dueDate)
        {
            if (dueDate == default)
                throw new DomainException(DomainExMsg.StudentInvoice_DueDateRequired);
            if (dueDate < IssueDate)
                throw new DomainException(DomainExMsg.StudentInvoice_DueDateBeforeIssueDate);
            DueDate = dueDate;
        }

        public void SetOriginalAmount(decimal originalAmount)
        {
            if (originalAmount < 0)
                throw new DomainException(DomainExMsg.StudentInvoice_OriginalAmountInvalid);
            var paidAmount = GetPaidAmount();
            OriginalAmount = originalAmount;
            RecalculateRemaining(paidAmount);
        }

        public void SetDiscountAmount(decimal discountAmount)
        {
            if (discountAmount < 0)
                throw new DomainException(DomainExMsg.StudentInvoice_DiscountAmountInvalid);
            if (discountAmount > OriginalAmount)
                throw new DomainException(DomainExMsg.StudentInvoice_DiscountExceedsOriginal);
            var paidAmount = GetPaidAmount();
            if (OriginalAmount - discountAmount < paidAmount)
                throw new DomainException(DomainExMsg.StudentInvoice_DiscountExceedsUnpaid);
            DiscountAmount = discountAmount;
            RecalculateRemaining(paidAmount);
        }

        public void SetStatus(enInvoiceStatus status)
        {
            Status = status;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }

        public void ApplyPayment(decimal paidAmount)
        {
            EnsureCanAcceptPayment();
            if (paidAmount <= 0)
                throw new DomainException(DomainExMsg.Payment_AmountMustBePositive);
            if (paidAmount > RemainingAmount)
                throw new DomainException(DomainExMsg.Payment_AmountExceedsRemaining);

            RecalculateRemaining(GetPaidAmount() + paidAmount);
        }

        public void ReversePayment(decimal paidAmount)
        {
            if (paidAmount <= 0)
                throw new DomainException(DomainExMsg.Payment_AmountMustBePositive);

            var nextPaid = GetPaidAmount() - paidAmount;
            if (nextPaid < 0)
                nextPaid = 0;
            RecalculateRemaining(nextPaid);
        }

        private decimal GetPaidAmount()
        {
            var paid = OriginalAmount - DiscountAmount - RemainingAmount;
            return paid < 0 ? 0 : paid;
        }

        private void RecalculateRemaining(decimal paidAmount)
        {
            RemainingAmount = OriginalAmount - DiscountAmount - paidAmount;
            if (RemainingAmount < 0)
                RemainingAmount = 0;
            RefreshStatus(paidAmount);
        }

        private void EnsureCanAcceptPayment()
        {
            if (Status == enInvoiceStatus.Cancelled)
                throw new DomainException(DomainExMsg.StudentInvoice_Cancelled);
        }

        private void RefreshStatus(decimal paidAmount)
        {
            if (Status == enInvoiceStatus.Cancelled)
                return;

            if (RemainingAmount <= 0)
                Status = enInvoiceStatus.Paid;
            else if (paidAmount > 0)
                Status = enInvoiceStatus.PartiallyPaid;
            else
                Status = enInvoiceStatus.Unpaid;
        }
    }
}
