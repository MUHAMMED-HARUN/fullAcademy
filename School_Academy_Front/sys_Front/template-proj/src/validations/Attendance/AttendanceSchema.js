import z from "zod";

export const AttendanceItemSchema = z.object({
  groupEnrollmentID: z
    .string()
    .uuid({ message: "معرّف عضوية المجموعة غير صالح" }),
  groupID: z.string().uuid({ message: "معرّف المجموعة غير صالح" }),
  status: z.number().int().min(1).max(4),
});

const attendanceDateSchema = z.string().regex(/^\d{4}-\d{2}-\d{2}$/, {
  message: "تاريخ الحضور غير صالح",
});

export const AttendanceBatchSchema = z
  .object({
    date: attendanceDateSchema,
    attendances: z
      .array(AttendanceItemSchema)
      .min(1, { message: "يجب اختيار عضو واحد على الأقل لتسجيل الحضور" })
      .max(500, { message: "لا يمكن إرسال أكثر من 500 عضوية في الطلب" }),
  })
  .superRefine((data, context) => {
    const groupIDs = new Set(data.attendances.map((item) => item.groupID));
    if (groupIDs.size > 1) {
      context.addIssue({
        code: "custom",
        path: ["attendances"],
        message: "يجب أن تخص دفعة الحضور مجموعة واحدة فقط",
      });
    }
    const enrollmentIDs = data.attendances.map(
      (item) => item.groupEnrollmentID
    );
    if (new Set(enrollmentIDs).size !== enrollmentIDs.length) {
      context.addIssue({
        code: "custom",
        path: ["attendances"],
        message: "لا يمكن تكرار عضوية المجموعة في دفعة الحضور",
      });
    }
  });

export const GroupAttendanceQuerySchema = z.object({
  groupID: z.string().uuid({ message: "معرّف المجموعة غير صالح" }),
  date: attendanceDateSchema,
});
