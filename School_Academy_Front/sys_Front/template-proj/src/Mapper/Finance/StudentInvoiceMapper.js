import { toDateInput } from "@/Generic/FinanceEnums";

export default class StudentInvoiceMapper {
  static mapNewToApi(dataSchema) {
    return {
      StudentAcademicEnrollmentID: dataSchema.studentAcademicEnrollmentID,
      FeeStructureID: dataSchema.feeStructureID,
      BillingPeriodStart: dataSchema.billingPeriodStart,
      BillingPeriodEnd: dataSchema.billingPeriodEnd,
      Notes: dataSchema.notes || null,
    };
  }

  static mapUpdateToApi(dataSchema) {
    return {
      StudentInvoiceID: dataSchema.studentInvoiceID,
      DiscountAmount: Number(dataSchema.discountAmount || 0),
      Notes: dataSchema.notes || null,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      studentInvoiceID: DataReq.StudentInvoiceID,
      studentID: DataReq.StudentID,
      studentName: DataReq.StudentName,
      studentAcademicEnrollmentID: DataReq.StudentAcademicEnrollmentID,
      academicPlanID: DataReq.AcademicPlanID,
      academicPlanName: DataReq.AcademicPlanName,
      academicPlanVersion: DataReq.AcademicPlanVersion,
      feeStructureID: DataReq.FeeStructureID,
      feeStructureName: DataReq.FeeStructureName,
      invoiceLabel: `${DataReq.StudentName || ""} — ${
        DataReq.AcademicPlanName || ""
      } / ${DataReq.FeeStructureName || ""} (متبقي ${
        DataReq.RemainingAmount ?? 0
      })`,
      billingPeriodStart: toDateInput(DataReq.BillingPeriodStart),
      billingPeriodEnd: toDateInput(DataReq.BillingPeriodEnd),
      issueDate: toDateInput(DataReq.IssueDate),
      dueDate: toDateInput(DataReq.DueDate),
      originalAmount: DataReq.OriginalAmount,
      discountAmount: DataReq.DiscountAmount,
      paidAmount: DataReq.PaidAmount,
      remainingAmount: DataReq.RemainingAmount,
      status: DataReq.Status,
      hasPayments: Boolean(DataReq.HasPayments),
      notes: DataReq.Notes,
    };
  }

  static mapListApiToClass(dataList) {
    if (!dataList) return [];
    const items = Array.isArray(dataList) ? dataList : [];
    return items.map((item) => this.MapFromApiDTO(item));
  }

  static mapPagingApiToClass(paging) {
    const source =
      paging?.Data ?? paging?.data ?? paging?.Items ?? paging?.items ?? [];
    return {
      items: this.mapListApiToClass(source),
      count:
        paging?.Count ??
        paging?.count ??
        paging?.TotalCount ??
        paging?.totalCount ??
        0,
    };
  }

  static mapFinanceSummary(data) {
    if (!data) return null;
    return {
      studentID: data.StudentID,
      studentName: data.StudentName,
      invoiceCount: data.InvoiceCount,
      totalOriginalAmount: data.TotalOriginalAmount,
      totalDiscountAmount: data.TotalDiscountAmount,
      totalPaidAmount: data.TotalPaidAmount,
      totalRemainingAmount: data.TotalRemainingAmount,
      overdueAmount: data.OverdueAmount,
    };
  }
}
