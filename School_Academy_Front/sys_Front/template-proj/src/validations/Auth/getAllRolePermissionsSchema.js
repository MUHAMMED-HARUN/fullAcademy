import { z } from "zod";

export const getAllRolePermissionsSchema = z.object({
  roleId: z.string().min(1, "يجب اختيار دور"),
});
