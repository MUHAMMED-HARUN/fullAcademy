import { z } from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";

const registerFields = z.object({
  userName: z
    .string()
    .trim()
    .min(4, "اسم المستخدم يجب ألا يقل عن 4 أحرف")
    .max(256, "اسم المستخدم طويل جداً"),
  email: z
    .string()
    .trim()
    .min(1, "البريد الإلكتروني مطلوب")
    .email("صيغة البريد غير صحيحة"),
  password: z
    .string()
    .min(8, "كلمة المرور يجب ألا تقل عن 8 أحرف")
    .regex(/[A-Z]/, "كلمة المرور يجب أن تحتوي حرفاً كبيراً")
    .regex(/[a-z]/, "كلمة المرور يجب أن تحتوي حرفاً صغيراً")
    .regex(/[0-9]/, "كلمة المرور يجب أن تحتوي رقماً")
    .regex(/[^a-zA-Z0-9]/, "كلمة المرور يجب أن تحتوي رمزاً خاصاً"),
  confirmPassword: z.string().min(1, "تأكيد كلمة المرور مطلوب"),
  roleID: GuidSchema,
  userType: z.union([z.literal(0), z.literal(1), z.literal(2)]),
  entityID: GuidSchema,
});

export const registerSchema = registerFields.superRefine((data, context) => {
  if (data.password !== data.confirmPassword) {
    context.addIssue({
      code: "custom",
      path: ["confirmPassword"],
      message: "كلمتا المرور غير متطابقتين",
    });
  }
});
