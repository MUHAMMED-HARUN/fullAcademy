import z from "zod";

export const TeachingAssignmentSchema = z.object({
  enrollmentID: z.string().optional(),
  teacherID: z.string().nonempty({ message: "مطلوب" }),
  groupID: z.string().nonempty({ message: "مطلوب" }),
  subjectID: z.string().nonempty({ message: "مطلوب" }),
  date: z.string().nonempty({ message: "تاريخ التسجيل مطلوب" }),
});
