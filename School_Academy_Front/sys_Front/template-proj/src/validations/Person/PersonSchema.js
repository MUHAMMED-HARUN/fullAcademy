import z from "zod";
import { addressSchema } from "@/validations/Address/addressSchema";
// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const PersonSchema = z.object({
  id: z.string().optional(),
  firstName: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  fatherName: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  lastName: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  motherName: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  motherLastName: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  nationalNumber: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  gender: z.boolean({ required_error: "  مطلوب" }),
  birthDate: z.coerce
    .date({
      required_error: "تاريخ الولادة مطلوب",
      invalid_type_error: "تاريخ غير صالح",
    })
    .min(new Date(1950, 0, 1), "يجب أن يكون أكبر من تاريخ 1950/01/01")
    .max(new Date(), "يجب أن يكون أقل من تاريخ اليوم"),

  placeOfBirthID: z.string({ required_error: "يجب اختيار " }),
  address: addressSchema,
});
