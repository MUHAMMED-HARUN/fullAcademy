import z from "zod";

// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const NeighborhoodSchema = z.object({
  id: z.string().optional(),
  name: z
    .string({ required_error: "اسم الشارع مطلوب" })
    .min(2, "اسم الشارع يجب أن يكون حرفين على الأقل")
    .max(100, "اسم الشارع طويل جداً"),
  districtID: z.string({ required_error: "يجب اختيار حارة" }),
});
