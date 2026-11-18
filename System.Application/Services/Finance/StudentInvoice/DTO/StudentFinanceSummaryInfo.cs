namespace System.Application.Services.Finance.StudentInvoice.DTO
{
    public sealed class StudentFinanceSummaryInfo
    {
        public string StudentID { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int InvoiceCount { get; set; }
        public decimal TotalOriginalAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalRemainingAmount { get; set; }
        public decimal OverdueAmount { get; set; }
    }
}
