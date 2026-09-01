import z from "zod";
export const AssignmentScopeSchema = z.object({
  assignmentScopeID: z.string().optional(),
  name: z.string().nonempty({ message: "مطلوب" }),
  note: z.string().optional({ message: "مطلوب" }),
  stageID: z.string().nonempty({ message: "مطلوب" }),
  prevAssignmentScopeID: z.string().optional({ message: "مطلوب" }),
});
