import z from "zod";

export const GroupSchema = z.object({
  groupID: z.string().optional(),
  groupName: z
    .string({ required_error: "مطلوب" })
    .min(2, "يجب أن يكون حرفين على الأقل")
    .max(100, "طويل جداً"),
  code: z
    .string({ required_error: "مطلوب" })
    .min(1, "مطلوب")
    .max(50, "طويل جداً"),
  instituteClassID: z.string().nonempty({ message: "مطلوب" }),
});
