using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.DTO
{
    public class StudentInvoiceInfo
    {
        public string StudentInvoiceID { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string? StudentAcademicEnrollmentID { get; set; }
        public string? AcademicPlanID { get; set; }
        public string? AcademicPlanName { get; set; }
        public int? AcademicPlanVersion { get; set; }
        public string FeeStructureID { get; set; }
        public string FeeStructureName { get; set; }
        public DateOnly? BillingPeriodStart { get; set; }
        public DateOnly? BillingPeriodEnd { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly? DueDate { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public enInvoiceStatus Status { get; set; }
        public string? Notes { get; set; }
        public bool HasPayments { get; set; }
    }
}
