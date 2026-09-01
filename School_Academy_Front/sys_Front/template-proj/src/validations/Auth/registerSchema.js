import { z } from "zod";

export const registerSchema = z
  .object({
    userName: z.string().min(3, "اسم المستخدم يجب أن يكون 3 أحرف على الأقل"),
    email: z
      .string()
      .min(1, "البريد الإلكتروني مطلوب")
      .email("صيغة البريد غير صحيحة"),
    entityID: z.string().min(1, "  مطلوب"),
    userType: z.number(),
    password: z.string().min(6, "كلمة المرور يجب أن لا تقل عن 6 أحرف"),
    confirmPassword: z.string().min(1, "تأكيد كلمة المرور مطلوب"),
    roleID: z
      .string({
        required_error: "معرف الدور (RoleId) مطلوب ولا يمكن أن يكون فارغاً.",
      })
      .trim()
      .min(1, {
        message: "معرف الدور (RoleId) مطلوب ولا يمكن أن يكون فارغاً.",
      }),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "كلمتا المرور غير متطابقتين",
    path: ["confirmPassword"],
  });
