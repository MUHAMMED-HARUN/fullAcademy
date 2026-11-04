import { toDateInput } from "@/Generic/FinanceEnums";

export default class StudentInvoiceMapper {
  static mapNewToApi(dataSchema) {
    return {
      StudentID: dataSchema.studentID,
      FeeStructureID: dataSchema.feeStructureID,
      IssueDate: dataSchema.issueDate,
      DueDate: dataSchema.dueDate,
      OriginalAmount: Number(dataSchema.originalAmount),
      DiscountAmount: Number(dataSchema.discountAmount || 0),
      Notes: dataSchema.notes || null,
    };
  }

  static mapUpdateToApi(dataSchema) {
    return {
      StudentInvoiceID: dataSchema.studentInvoiceID,
      DueDate: dataSchema.dueDate,
      DiscountAmount: Number(dataSchema.discountAmount || 0),
      Status: Number(dataSchema.status),
      Notes: dataSchema.notes || null,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      studentInvoiceID: DataReq.StudentInvoiceID,
      studentID: DataReq.StudentID,
      studentName: DataReq.StudentName,
      feeStructureID: DataReq.FeeStructureID,
      feeStructureName: DataReq.FeeStructureName,
      issueDate: toDateInput(DataReq.IssueDate),
      dueDate: toDateInput(DataReq.DueDate),
      originalAmount: DataReq.OriginalAmount,
      discountAmount: DataReq.DiscountAmount,
      remainingAmount: DataReq.RemainingAmount,
      status: DataReq.Status,
      notes: DataReq.Notes,
    };
  }

  static mapListApiToClass(dataList) {
    if (!dataList) return [];
    const items = Array.isArray(dataList) ? dataList : [];
    return items.map((item) => this.MapFromApiDTO(item));
  }
}
