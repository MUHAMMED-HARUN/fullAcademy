import z from "zod";

export const StudentEnrollmentSchema = z.object({
  studentAcademicEnrollmentID: z
    .string()
    .uuid({ message: "اختر اشتراكاً أكاديمياً صالحاً" }),
  groupID: z.string().uuid({ message: "اختر مجموعة صالحة" }),
  date: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});

export const StudentEnrollmentUpdateSchema = z.object({
  groupEnrollmentID: z
    .string()
    .uuid({ message: "معرّف عضوية المجموعة غير صالح" }),
  date: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});

export const StudentEnrollmentRangeSchema = z.object({
  studentAcademicEnrollmentIDs: z
    .array(z.string().uuid({ message: "اختر اشتراكات أكاديمية صالحة" }))
    .min(1, "اختر اشتراكاً أكاديمياً واحداً على الأقل"),
  groupID: z.string().uuid({ message: "اختر مجموعة صالحة" }),
  date: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});
