import z from "zod";
export const StageSchema = z.object({
  stageID: z.union([z.string().uuid(), z.literal("")]).optional(),
  name: z
    .string()
    .trim()
    .min(1, { message: "مطلوب" })
    .max(100, { message: "الحد الأقصى 100 حرف" }),
  note: z
    .string()
    .max(500, { message: "الحد الأقصى 500 حرف" })
    .optional()
    .nullable(),
  classID: z
    .string()
    .regex(/^[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}$/i, {
      message: "يجب اختيار صف صالح",
    })
    .refine(
      (value) => value.toLowerCase() !== "00000000-0000-0000-0000-000000000000",
      { message: "يجب اختيار صف صالح" }
    ),
  prevStageID: z.union([z.string().uuid(), z.literal(""), z.null()]).optional(),
});
