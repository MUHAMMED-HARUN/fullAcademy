import z from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";

const districtFields = {
  name: z
    .string({ required_error: "اسم الحارة مطلوب" })
    .trim()
    .min(2, "اسم الحارة يجب أن يكون حرفين على الأقل")
    .max(100, "اسم الحارة طويل جداً"),
  cityID: GuidSchema,
};

export const NewDistrictSchema = z.object({
  id: z.literal("").optional(),
  ...districtFields,
});

export const UpdateDistrictSchema = z.object({
  id: GuidSchema,
  ...districtFields,
});

export const DistrictSchema = z.union([
  NewDistrictSchema,
  UpdateDistrictSchema,
]);

export const DistrictLookupSchema = z.object({
  cityID: GuidSchema,
  value: z
    .string()
    .trim()
    .max(100, "قيمة البحث طويلة جداً")
    .optional()
    .default("")
    .refine(
      (value) =>
        ![",", "|", "=", "*", "(", ")", "^", "$", "!", "/"].some((token) =>
          value.includes(token)
        ),
      { message: "تحتوي قيمة البحث على رموز غير مسموحة" }
    ),
});
