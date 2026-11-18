import z from "zod";
export const StudentStageSchema = z.object({
  studentStageID: z.union([z.literal(""), z.string().uuid()]).optional(),
  studentAcademicEnrollmentID: z
    .string()
    .uuid({ message: "اختر اشتراكاً أكاديمياً صالحاً" }),
  stageID: z.string().uuid({ message: "اختر مرحلة ضمن الخطة" }),
  studentStageState: z.number().int().min(0).max(3),
  studentStageDate: z
    .string()
    .regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});

export const StudentStageUpdateSchema = z.object({
  studentStageID: z.string().uuid({ message: "معرّف المرحلة غير صالح" }),
  studentStageState: z.number().int().min(0).max(3),
  studentStageDate: z
    .string()
    .regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});
