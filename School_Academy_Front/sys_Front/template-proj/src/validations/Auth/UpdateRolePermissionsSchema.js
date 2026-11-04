import { z } from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";

const permissionNames = [
  "Can_Update_User",
  "Can_Read_User",
  "Can_SetPermission_User",
  "Can_Register_User",
  "Can_Delete_User",
  "Can_Read_Country",
  "Can_Create_Country",
  "Can_Update_Country",
  "Can_Delete_Country",
  "Can_Read_City",
  "Can_Create_City",
  "Can_Update_City",
  "Can_Delete_City",
  "Can_Read_District",
  "Can_Create_District",
  "Can_Update_District",
  "Can_Delete_District",
  "Can_Read_Neighborhood",
  "Can_Create_Neighborhood",
  "Can_Update_Neighborhood",
  "Can_Delete_Neighborhood",
  "Can_Read_Address",
  "Can_Create_Address",
  "Can_Update_Address",
  "Can_Delete_Address",
  "Can_Read_Institute",
  "Can_Create_Institute",
  "Can_Update_Institute",
  "Can_Delete_Institute",
  "Can_Read_Class",
  "Can_Create_Class",
  "Can_Update_Class",
  "Can_Delete_Class",
  "Can_Read_InstituteClass",
  "Can_Create_InstituteClass",
  "Can_Update_InstituteClass",
  "Can_Delete_InstituteClass",
];

// 1. مخطط فحص العنصر الفردي (RolePermissionChangeDto)
export const rolePermissionChangeDtoSchema = z.object({
  roleId: GuidSchema,

  permissionName: z
    .string()
    .refine((value) => permissionNames.includes(value), {
      message: "الصلاحية غير معروفة",
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
