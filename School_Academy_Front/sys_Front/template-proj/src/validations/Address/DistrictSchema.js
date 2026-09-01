import z from "zod";

// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const DistrictSchema = z.object({
  id: z.string().optional(),
  name: z
    .string({ required_error: "اسم الحارة مطلوب" })
    .min(2, "اسم الحارة يجب أن يكون حرفين على الأقل")
    .max(100, "اسم الحارة طويل جداً"),
  cityID: z.string({ required_error: "يجب اختيار مدينة" }),
});
