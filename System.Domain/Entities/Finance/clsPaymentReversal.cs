using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Finance
{
    public sealed class clsPaymentReversal : clsBaseClass
    {
        public Guid PaymentID { get; private set; }
        public decimal Amount { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public string ReversedBy { get; private set; } = string.Empty;
        public DateTime ReversedAt { get; private set; }
        public clsPayment Payment { get; private set; } = null!;

        private clsPaymentReversal() { }

        public clsPaymentReversal(Guid paymentID, decimal amount, string reason, string reversedBy)
        {
            if (paymentID == Guid.Empty || amount <= 0 || string.IsNullOrWhiteSpace(reason) ||
                reason.Trim().Length > 500 || string.IsNullOrWhiteSpace(reversedBy))
                throw new DomainException(DomainExMsg.PaymentReversal_Invalid);

            PaymentID = paymentID;
            Amount = amount;
            Reason = reason.Trim();
            ReversedBy = reversedBy;
            ReversedAt = DateTime.UtcNow;
            BaseSetCreated(reversedBy);
        }

        public override void SetModified(string userId) =>
            throw new DomainException(DomainExMsg.PaymentReversal_Immutable);

        public override void SetDeleted(string userId) =>
            throw new DomainException(DomainExMsg.PaymentReversal_Immutable);
    }
}
