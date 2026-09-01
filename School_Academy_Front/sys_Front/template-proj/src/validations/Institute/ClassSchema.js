import z from "zod";
// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const ClassSchema = z.object({
  classID: z.string().optional(),
  name: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  notes: z.string({ required_error: "  مطلوب" }).max(100, "  طويل جداً"),
  level: z
    .number({ required_error: "  مطلوب" })
    .min(0, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  programID: z.string().nonempty({ message: "  مطلوب" }),
});
