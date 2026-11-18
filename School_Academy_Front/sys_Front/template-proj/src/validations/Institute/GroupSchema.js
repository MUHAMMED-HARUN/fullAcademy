import z from "zod";

export const GroupSchema = z.object({
  groupID: z.union([z.literal(""), z.string().uuid()]).optional(),
  groupName: z
    .string({ required_error: "مطلوب" })
    .min(2, "يجب أن يكون حرفين على الأقل")
    .max(150, "يجب ألا يتجاوز 150 حرفاً"),
  code: z
    .string({ required_error: "مطلوب" })
    .min(1, "مطلوب")
    .max(50, "طويل جداً"),
  instituteClassID: z.string().uuid({ message: "اختر صف معهد صالحاً" }),
});
