import z from "zod";
export const AssignmentNominationSchema = z.object({
  assignmentNominationID: z
    .union([z.literal(""), z.string().uuid()])
    .optional(),
  studentAcademicEnrollmentID: z
    .string()
    .uuid({ message: "اختر اشتراكاً أكاديمياً صالحاً" }),
  assignmentScopeID: z.string().uuid({ message: "اختر نطاق اختبار صالحاً" }),
  date: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});
