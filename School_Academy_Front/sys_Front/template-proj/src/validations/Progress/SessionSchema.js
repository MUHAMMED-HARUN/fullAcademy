import z from "zod";

export const SessionSchema = z.object({
  sessionID: z.string().optional(),
  name: z
    .string()
    .trim()
    .min(1, { message: "مطلوب" })
    .max(100, { message: "الحد الأقصى 100 حرف" }),
  type: z.coerce.number().int().min(0).max(1),
  note: z
    .string()
    .max(500, { message: "الحد الأقصى 500 حرف" })
    .optional()
    .nullable(),
  parentSessionID: z
    .union([
      z.string().uuid({ message: "اختر موضوعاً صالحاً" }),
      z.literal(""),
      z.null(),
    ])
    .optional(),
});
