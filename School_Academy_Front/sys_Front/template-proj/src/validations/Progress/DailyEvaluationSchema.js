import z from "zod";
export const DailyEvaluationSchema = z.object({
  dailyEvaluationID: z.union([z.literal(""), z.string().uuid()]).optional(),
  studentAcademicEnrollmentID: z
    .string()
    .uuid({ message: "اختر اشتراكاً أكاديمياً صالحاً" }),
  to: z.number().finite().min(0, "يجب أن تكون النهاية صفراً أو أكبر"),
  date: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
  stageInfoID: z.string().uuid({ message: "اختر تفاصيل مرحلة صالحة" }),
  levelType: z.number().int().min(0).max(4),
});
