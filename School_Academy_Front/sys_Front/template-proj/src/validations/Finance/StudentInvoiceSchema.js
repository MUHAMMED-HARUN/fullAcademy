import z from "zod";

const dateOnly = z.string().regex(/^\d{4}-\d{2}-\d{2}$/, {
  message: "التاريخ غير صالح",
});

export const GenerateStudentInvoiceSchema = z
  .object({
    studentAcademicEnrollmentID: z
      .string()
      .uuid({ message: "التسجيل الأكاديمي مطلوب" }),
    feeStructureID: z.string().uuid({ message: "التعرفة مطلوبة" }),
    billingPeriodStart: dateOnly,
    billingPeriodEnd: dateOnly,
    notes: z.string().optional().nullable(),
  })
  .refine((data) => data.billingPeriodStart <= data.billingPeriodEnd, {
    message: "نهاية فترة الفوترة يجب ألا تسبق بدايتها",
    path: ["billingPeriodEnd"],
  });

export const UpdateStudentInvoiceSchema = z.object({
  studentInvoiceID: z.string().uuid({ message: "معرف الفاتورة غير صالح" }),
  discountAmount: z.coerce.number().min(0, {
    message: "الخصم يجب أن يكون أكبر من أو يساوي 0",
  }),
  notes: z.string().optional().nullable(),
});
