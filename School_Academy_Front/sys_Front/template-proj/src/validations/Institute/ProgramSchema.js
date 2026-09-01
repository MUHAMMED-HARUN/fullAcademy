import z from "zod";
export const ProgramuteSchema = z.object({
  programID: z.string().optional(),
  name: z
    .string({ required_error: "  مطلوب" })
    .min(2, "  يجب أن يكون حرفين على الأقل")
    .max(100, "  طويل جداً"),
  note: z.string({ required_error: "مطلوب" }).max(100, "  طويل جداً"),
});
