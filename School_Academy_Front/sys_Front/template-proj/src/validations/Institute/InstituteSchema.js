import z from "zod";
import { addressSchema } from "@/validations/Address/addressSchema";
// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const InstituteSchema = z.object({
  instituteID: z.string().optional(),
  name: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  notes: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),

  address: addressSchema,
});
