namespace System.Domain.Enums.Finance
{
    public enum enInvoiceStatus
    {
        Unpaid = 1,
        PartiallyPaid = 2,
        Paid = 3,
        Cancelled = 4,
        Overdue = 5
    }

    public enum enFeeType
    {
        Tuition = 1,
        Registration = 2,
        Exam = 3,
        Material = 4,
        Other = 5
    }

    public enum enPaymentMethod
    {
        Cash = 1,
        BankTransfer = 2,
        CreditCard = 3,
        Cheque = 4,
        Online = 5
    }

    public enum enFeeStructureParamType { ID, Name, ProgramID, ClassID, StageID }
    public enum enStudentInvoiceParamType { ID, StudentID, FeeStructureID, Status }
    public enum enPaymentParamType { ID, StudentInvoiceID, ReferenceNumber }
}
