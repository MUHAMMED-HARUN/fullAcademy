import z from "zod";

// مخطط التحقق لنماذج إضافة/تعديل الدولة
export const addressSchema = z.object({
  id: z.string().optional(),
  countryID: z.string({ required_error: "يجب اختيار دولة" }),
  cityID: z.string({ required_error: "يجب اختيار مدينة" }),
  districtID: z.string({ required_error: "يجب اختيار حارة" }),
  neighborhoodID: z.string({ required_error: "يجب اختيار شارع" }),
  notes: z
    .string()
    .max(100, "لا يمكن ان تكون الملاحظات اكثر من 100 حرف")
    .optional()
    .nullable(),
});
