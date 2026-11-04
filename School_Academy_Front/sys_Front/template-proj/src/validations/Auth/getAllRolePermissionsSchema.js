import { z } from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";

export const getAllRolePermissionsSchema = z.object({
  roleId: GuidSchema,
});
