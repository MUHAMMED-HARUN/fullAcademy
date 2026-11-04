import { z } from "zod";

export const loginSchema = z.object({
  email: z
    .string()
    .min(1, "البريد الإلكتروني مطلوب")
    .email("صيغة البريد غير صحيحة"),
  password: z.string().min(1, "كلمة المرور مطلوبة"),
});
