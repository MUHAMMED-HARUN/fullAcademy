import { toDateTimeInput } from "@/Generic/FinanceEnums";

export default class PaymentMapper {
  static mapNewToApi(data) {
    return {
      StudentInvoiceID: data.studentInvoiceID,
      Amount: Number(data.amount),
      PaymentDate: data.paymentDate,
      PaymentMethod: Number(data.paymentMethod),
      ReferenceNumber: data.referenceNumber || null,
      Notes: data.notes || null,
    };
  }

  static mapReverseToApi(data) {
    return {
      PaymentID: data.paymentID,
      Reason: data.reason,
    };
  }

  static MapFromApiDTO(data) {
    if (!data) return null;
    return {
      paymentID: data.PaymentID,
      studentInvoiceID: data.StudentInvoiceID,
      studentAcademicEnrollmentID: data.StudentAcademicEnrollmentID,
      studentName: data.StudentName,
      academicPlanName: data.AcademicPlanName,
      feeStructureName: data.FeeStructureName,
      invoiceLabel: data.InvoiceLabel,
      amount: data.Amount,
      paymentDate: toDateTimeInput(data.PaymentDate),
      paymentMethod: data.PaymentMethod,
      referenceNumber: data.ReferenceNumber,
      notes: data.Notes,
      isReversed: Boolean(data.IsReversed),
      reversalReason: data.ReversalReason,
      reversedAt: data.ReversedAt ? toDateTimeInput(data.ReversedAt) : null,
      reversedBy: data.ReversedBy,
    };
  }

  static mapListApiToClass(data) {
    return Array.isArray(data) ? data.map((item) => this.MapFromApiDTO(item)) : [];
  }

  static mapPagingApiToClass(paging) {
    const source = paging?.Data ?? paging?.data ?? paging?.Items ?? paging?.items ?? [];
    return {
      items: this.mapListApiToClass(source),
      count: paging?.Count ?? paging?.count ?? paging?.TotalCount ?? paging?.totalCount ?? 0,
    };
  }
}
