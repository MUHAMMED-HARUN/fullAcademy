import z from "zod";
export const StudentStageSchema = z.object({
  stageID: z.string().nonempty({ message: "مطلوب" }),
  enrollmentStudentID: z.string().nonempty({ message: "مطلوب" }),
});
