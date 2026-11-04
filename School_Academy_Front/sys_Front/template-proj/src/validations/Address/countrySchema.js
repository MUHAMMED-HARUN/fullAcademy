import z from "zod";

const countryFields = {
  name: z
    .string({ required_error: "اسم الدولة مطلوب" })
    .trim()
    .min(2, "اسم الدولة يجب أن يكون حرفين على الأقل")
    .max(100, "اسم الدولة طويل جداً"),
  code: z
    .string({ required_error: "رمز الدولة مطلوب" })
    .trim()
    .min(1, "رمز الدولة مطلوب")
    .max(10, "رمز الدولة طويل جداً"),
};

export const NewCountrySchema = z.object({
  id: z.literal("").optional(),
  ...countryFields,
});

export const UpdateCountrySchema = z.object({
  id: z
    .string()
    .regex(/^[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}$/i, {
      message: "معرّف الدولة غير صالح",
    })
    .refine(
      (id) => id.toLowerCase() !== "00000000-0000-0000-0000-000000000000",
      {
        message: "يجب اختيار دولة صالحة",
      }
    ),
  ...countryFields,
});

export const countrySchema = z.union([NewCountrySchema, UpdateCountrySchema]);
