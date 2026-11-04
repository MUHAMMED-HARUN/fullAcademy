import z from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";
// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const InstituteClassSchema = z.object({
  instituteClassID: z.string().optional(),
  classID: z.string().nonempty({ message: "  مطلوب" }),
  instituteID: z.string().nonempty({ message: "  مطلوب" }),
});

export const InstituteClassByRangeSchema = z.object({
  instituteID: GuidSchema,
  classIDs: z
    .array(GuidSchema)
    .min(1, { message: "اختر صفاً واحداً على الأقل" })
    .refine(
      (classIDs) =>
        new Set(classIDs.map((id) => id.toLowerCase())).size ===
        classIDs.length,
      {
        message: "توجد صفوف مكررة في القائمة",
      }
    ),
});
