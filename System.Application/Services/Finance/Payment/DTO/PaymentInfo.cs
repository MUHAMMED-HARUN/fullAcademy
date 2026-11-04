using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.DTO
{
    public class PaymentInfo
    {
        public string PaymentID { get; set; }
        public string StudentInvoiceID { get; set; }
        public string StudentName { get; set; }
        public string FeeStructureName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
    }
}
