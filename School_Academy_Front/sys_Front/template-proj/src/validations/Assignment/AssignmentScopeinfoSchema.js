import z from "zod";
export const AssignmentScopeInfoSchema = z.object({
  assignmentScopeInfoID: z.string().optional(),
  maxScore: z.number().min(0, { message: "مطلوب" }),
  minPassScore: z.number().min(0, { message: "مطلوب" }),
  weight: z.number().min(0, { message: "مطلوب" }),
  stageInfoID: z.string().optional({ message: "مطلوب" }),
  assignmentScopeID: z.string().optional({ message: "مطلوب" }),
});
