import z from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";

const neighborhoodFields = {
  name: z
    .string({ required_error: "اسم الشارع مطلوب" })
    .trim()
    .min(2, "اسم الشارع يجب أن يكون حرفين على الأقل")
    .max(100, "اسم الشارع طويل جداً"),
  districtID: GuidSchema,
};

export const NewNeighborhoodSchema = z.object({
  id: z.literal("").optional(),
  ...neighborhoodFields,
});

export const UpdateNeighborhoodSchema = z.object({
  id: GuidSchema,
  ...neighborhoodFields,
});

export const NeighborhoodSchema = z.union([
  NewNeighborhoodSchema,
  UpdateNeighborhoodSchema,
]);

export const NeighborhoodLookupSchema = z.object({
  districtID: GuidSchema,
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
