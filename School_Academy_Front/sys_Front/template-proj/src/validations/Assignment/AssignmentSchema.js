import z from "zod";
export const AssignmentSchema = z.object({
  assignmentID: z.string().optional(),
  assignmentNominationID: z.string().nonempty({ message: "مطلوب" }),
});
