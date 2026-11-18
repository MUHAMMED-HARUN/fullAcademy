using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Finance
{
    public class clsPayment : clsBaseClass
    {
        public Guid StudentInvoiceID { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public enPaymentMethod PaymentMethod { get; private set; }
        public string? ReferenceNumber { get; private set; }
        public string? Notes { get; private set; }

        // Navigation properties
        public clsStudentInvoice StudentInvoice { get; private set; }
        public clsPaymentReversal? Reversal { get; private set; }

        protected clsPayment() { }

        public clsPayment(
            Guid studentInvoiceID,
            decimal amount,
            DateTime paymentDate,
            enPaymentMethod paymentMethod,
            string? referenceNumber,
            string? notes)
        {
            SetStudentInvoiceID(studentInvoiceID);
            SetAmount(amount);
            SetPaymentDate(paymentDate);
            SetPaymentMethod(paymentMethod);
            SetReferenceNumber(referenceNumber);
            SetNotes(notes);
            BaseSetCreated("System");
        }

        private void SetStudentInvoiceID(Guid studentInvoiceID)
        {
            if (studentInvoiceID == Guid.Empty)
                throw new DomainException(DomainExMsg.Payment_StudentInvoiceIDRequired);
            StudentInvoiceID = studentInvoiceID;
        }

        private void SetAmount(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException(DomainExMsg.Payment_AmountMustBePositive);
            Amount = amount;
        }

        private void SetPaymentDate(DateTime paymentDate)
        {
            if (paymentDate == default)
                PaymentDate = DateTime.Now;
            else
                PaymentDate = paymentDate;
        }

        private void SetPaymentMethod(enPaymentMethod paymentMethod)
        {
            PaymentMethod = paymentMethod;
        }

        private void SetReferenceNumber(string? referenceNumber)
        {
            ReferenceNumber = referenceNumber;
        }

        private void SetNotes(string? notes)
        {
            Notes = notes;
        }

        public override void SetModified(string userId) =>
            throw new DomainException(DomainExMsg.Payment_Immutable);

        public override void SetDeleted(string userId) =>
            throw new DomainException(DomainExMsg.Payment_Immutable);
    }
}
