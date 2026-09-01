import z from "zod";

// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const citySchema = z.object({
  id: z.string().optional(),
  name: z
    .string({ required_error: "اسم المدينة مطلوب" })
    .min(2, "اسم المدينة يجب أن يكون حرفين على الأقل")
    .max(100, "اسم المدينة طويل جداً"),
  cityCode: z
    .string()
    .max(10, "رمز المدينة يجب ألا يتجاوز 10 أحرف")
    .optional()
    .nullable(),
  countryID: z.string({ required_error: "يجب اختيار مدينة" }),
});
