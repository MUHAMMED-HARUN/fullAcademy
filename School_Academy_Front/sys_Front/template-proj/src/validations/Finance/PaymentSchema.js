import z from "zod";

export const PaymentSchema = z.object({
  studentInvoiceID: z.string().uuid({ message: "الفاتورة غير صالحة" }),
  amount: z.coerce.number({ message: "المبلغ مطلوب" }).gt(0, {
    message: "المبلغ يجب أن يكون أكبر من 0",
  }),
  paymentDate: z.string().min(1, { message: "تاريخ الدفع مطلوب" }),
  paymentMethod: z.coerce.number({ message: "طريقة الدفع مطلوبة" }).min(1),
  referenceNumber: z.string().optional().nullable(),
  notes: z.string().optional().nullable(),
});

export const ReversePaymentSchema = z.object({
  paymentID: z.string().uuid({ message: "الدفعة غير صالحة" }),
  reason: z.string().trim().min(1, { message: "سبب العكس مطلوب" }).max(500, {
    message: "سبب العكس يجب ألا يتجاوز 500 حرف",
  }),
});
