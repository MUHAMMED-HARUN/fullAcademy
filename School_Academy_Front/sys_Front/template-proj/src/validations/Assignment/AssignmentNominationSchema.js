import z from "zod";
export const AssignmentNominationSchema = z.object({
  assignmentNominationID: z.string().optional(),
  studentEnrollmentID: z.string().nonempty({ message: "مطلوب" }),
  assignmentScopeID: z.string().nonempty({ message: "مطلوب" }),
  date: z.string().nonempty({ message: "مطلوب" }),
});
