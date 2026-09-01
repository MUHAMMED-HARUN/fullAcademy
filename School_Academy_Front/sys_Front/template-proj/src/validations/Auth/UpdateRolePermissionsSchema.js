import { z } from "zod";

// 1. مخطط فحص العنصر الفردي (RolePermissionChangeDto)
export const rolePermissionChangeDtoSchema = z.object({
  roleId: z
    .string({
      required_error: "معرف الدور (RoleId) مطلوب ولا يمكن أن يكون فارغاً.",
    })
    .trim()
    .min(1, { message: "معرف الدور (RoleId) مطلوب ولا يمكن أن يكون فارغاً." }),

  permissionName: z
    .string({
      required_error:
        "اسم الصلاحية (PermissionName) مطلوب ولا يمكن أن يكون فارغاً.",
    })
    .trim()
    .min(1, {
      message: "اسم الصلاحية (PermissionName) مطلوب ولا يمكن أن يكون فارغاً.",
    }),

  isDelete: z.boolean(),
});

// 2. مخطط فحص الأمر الكامل (UpdateRolePermissionsCommand)
export const updateRolePermissionsSchema = z.object({
  changes: z
    .array(rolePermissionChangeDtoSchema, {
      required_error: "قائمة التغييرات لا يمكن أن تكون null.",
      invalid_type_error: "قائمة التغييرات لا يمكن أن تكون null.",
    })
    .min(1, { message: "يجب إرسال عنصر واحد على الأقل داخل قائمة التغييرات." }),
});
