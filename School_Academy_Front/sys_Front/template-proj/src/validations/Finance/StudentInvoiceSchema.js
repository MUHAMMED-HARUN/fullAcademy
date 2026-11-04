import z from "zod";

export const NewStudentInvoiceSchema = z.object({
  studentID: z.string().nonempty({ message: "الطالب مطلوب" }),
  feeStructureID: z.string().nonempty({ message: "خطة الرسوم مطلوبة" }),
  issueDate: z.string().nonempty({ message: "تاريخ الإصدار مطلوب" }),
  dueDate: z.string().nonempty({ message: "تاريخ الاستحقاق مطلوب" }),
  originalAmount: z.coerce.number({ message: "المبلغ الأصلي مطلوب" }).min(0, {
    message: "المبلغ الأصلي يجب أن يكون أكبر من أو يساوي 0",
  }),
  discountAmount: z.coerce.number().min(0, {
    message: "الخصم يجب أن يكون أكبر من أو يساوي 0",
  }),
  notes: z.string().optional().nullable(),
});

export const UpdateStudentInvoiceSchema = z.object({
  studentInvoiceID: z.string().nonempty({ message: "معرف الفاتورة مطلوب" }),
  dueDate: z.string().nonempty({ message: "تاريخ الاستحقاق مطلوب" }),
  discountAmount: z.coerce.number().min(0, {
    message: "الخصم يجب أن يكون أكبر من أو يساوي 0",
  }),
  status: z.coerce.number({ message: "الحالة مطلوبة" }).min(1, {
    message: "الحالة مطلوبة",
  }),
  notes: z.string().optional().nullable(),
});
