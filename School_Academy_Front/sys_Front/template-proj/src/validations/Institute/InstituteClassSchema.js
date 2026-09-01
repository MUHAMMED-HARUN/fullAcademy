import z from "zod";
// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const InstituteClassSchema = z.object({
  instituteClassID: z.string().optional(),
  classID: z.string().nonempty({ message: "  مطلوب" }),
  instituteID: z.string().nonempty({ message: "  مطلوب" }),
});
