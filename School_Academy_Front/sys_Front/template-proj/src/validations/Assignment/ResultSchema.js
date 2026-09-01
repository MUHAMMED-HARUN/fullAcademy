import z from "zod";
export const ResultSchema = z.object({
  resultID: z.string().optional(),
  assignmentID: z.string().nonempty({ message: "مطلوب" }),
  assignmentScopeInfoID: z.string().nonempty({ message: "مطلوب" }),
  score: z.number("  مطلوب").min(0, "  يجب أن يكون اكبر من 0"),
  notes: z.string().optional({ message: "مطلوب" }),
});
