import z from "zod";

export const PaymentSchema = z.object({
  paymentID: z.string().optional().nullable(),
  studentInvoiceID: z.string().nonempty({ message: "الفاتورة مطلوبة" }),
  amount: z.coerce.number({ message: "المبلغ مطلوب" }).gt(0, {
    message: "المبلغ يجب أن يكون أكبر من 0",
  }),
  paymentDate: z.string().nonempty({ message: "تاريخ الدفع مطلوب" }),
  paymentMethod: z.coerce.number({ message: "طريقة الدفع مطلوبة" }).min(1, {
    message: "طريقة الدفع مطلوبة",
  }),
  referenceNumber: z.string().optional().nullable(),
  notes: z.string().optional().nullable(),
});
