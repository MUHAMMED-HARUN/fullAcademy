import z from "zod";
export const AssignmentSchema = z.object({
  assignmentNominationID: z.string().uuid({ message: "الترشيح غير صالح" }),
  scheduledDate: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});

export const AssignmentUpdateSchema = z.object({
  assignmentID: z.string().uuid({ message: "الاختبار غير صالح" }),
  scheduledDate: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً"),
});
