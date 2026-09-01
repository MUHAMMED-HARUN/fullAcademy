import z from "zod";
export const DailyEvaluationSchema = z.object({
  dailyEvaluationID: z.string().optional(),
  studentEnrollmentID: z.string().optional("مطلوب"),
  to: z.number("  مطلوب").min(0, "  يجب أن يكون اكبر من 0"),
  date: z.string().nonempty({ message: "تاريخ التسجيل مطلوب" }),
  stageInfoID: z.string().optional("مطلوب"),
  levelType: z.number("  مطلوب").min(0, "  يجب أن يكون حرفين على الأقل"),
});
