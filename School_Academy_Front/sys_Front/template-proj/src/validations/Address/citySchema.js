import z from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";

const cityFields = {
  name: z
    .string({ required_error: "اسم المدينة مطلوب" })
    .trim()
    .min(2, "اسم المدينة يجب أن يكون حرفين على الأقل")
    .max(100, "اسم المدينة طويل جداً"),
  code: z
    .string()
    .trim()
    .max(10, "رمز المدينة يجب ألا يتجاوز 10 أحرف")
    .optional()
    .nullable(),
  countryID: GuidSchema,
};

export const NewCitySchema = z.object({
  id: z.literal("").optional(),
  ...cityFields,
});

export const UpdateCitySchema = z.object({
  id: GuidSchema,
  ...cityFields,
});

export const citySchema = z.union([NewCitySchema, UpdateCitySchema]);

export const CityLookupSchema = z.object({
  countryID: GuidSchema,
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
      {
        message: "تحتوي قيمة البحث على رموز غير مسموحة",
      }
    ),
});
