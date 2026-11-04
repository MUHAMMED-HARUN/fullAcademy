import { toDateTimeInput } from "@/Generic/FinanceEnums";

export default class PaymentMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      PaymentID: dataSchema.paymentID || undefined,
      StudentInvoiceID: dataSchema.studentInvoiceID,
      Amount: Number(dataSchema.amount),
      PaymentDate: dataSchema.paymentDate,
      PaymentMethod: Number(dataSchema.paymentMethod),
      ReferenceNumber: dataSchema.referenceNumber || null,
      Notes: dataSchema.notes || null,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      paymentID: DataReq.PaymentID,
      studentInvoiceID: DataReq.StudentInvoiceID,
      amount: DataReq.Amount,
      paymentDate: toDateTimeInput(DataReq.PaymentDate),
      paymentMethod: DataReq.PaymentMethod,
      referenceNumber: DataReq.ReferenceNumber,
      notes: DataReq.Notes,
    };
  }

  static mapListApiToClass(dataList) {
    if (!dataList) return [];
    const items = Array.isArray(dataList) ? dataList : [];
    return items.map((item) => this.MapFromApiDTO(item));
  }
}
