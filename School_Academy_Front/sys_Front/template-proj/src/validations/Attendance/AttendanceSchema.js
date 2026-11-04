import z from "zod";

export const AttendanceItemSchema = z.object({
  entityID: z.string().min(1, { message: "معرف الكيان مطلوب" }),
  attendeeType: z.number().int().min(1),
  status: z.number().int().min(1).max(4),
  contextID: z.string().nullable().optional(),
  contextType: z.number().int().nullable().optional(),
});

export const AttendanceBatchSchema = z.array(AttendanceItemSchema).min(1, {
  message: "يجب اختيار طالب واحد على الأقل لتسجيل الحضور",
});
