import z from "zod";

export const InstituteLookupSchema = z.object({
  value: z
    .string()
    .trim()
    .min(1, "أدخل اسم المعهد للبحث")
    .max(100, "قيمة البحث طويلة جداً")
    .refine(
      (value) =>
        ![",", "|", "=", "*", "(", ")", "^", "$", "!", "/"].some((token) =>
          value.includes(token)
        ),
      { message: "تحتوي قيمة البحث على رموز غير مسموحة" }
    ),
});
