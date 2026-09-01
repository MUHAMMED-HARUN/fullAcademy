import z from "zod";

// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const countrySchema = z.object({
  id: z.string().optional(),
  Name: z
    .string({ required_error: "اسم الدولة مطلوب" })
    .min(2, "اسم الدولة يجب أن يكون حرفين على الأقل")
    .max(100, "اسم الدولة طويل جداً"),
  Code: z
    .string()
    .max(10, "رمز الدولة يجب ألا يتجاوز 10 أحرف")
    .optional()
    .nullable(),
});
