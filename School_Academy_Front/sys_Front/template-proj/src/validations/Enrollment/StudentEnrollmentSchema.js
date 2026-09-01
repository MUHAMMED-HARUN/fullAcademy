import z from "zod";

export const StudentEnrollmentSchema = z.object({
  enrollmentID: z.string().optional(),
  studentID: z.string().nonempty({ message: "مطلوب" }),
  groupID: z.string().nonempty({ message: "مطلوب" }),
  date: z.string().nonempty({ message: "تاريخ التسجيل مطلوب" }),
});
