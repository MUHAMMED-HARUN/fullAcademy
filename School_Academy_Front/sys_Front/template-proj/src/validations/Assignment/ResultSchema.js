import z from "zod";

const ResultFields = z.object({
  resultID: z.union([z.literal(""), z.string().uuid()]).optional(),
  assignmentID: z.string().uuid({ message: "الاختبار غير صالح" }),
  assignmentScopeInfoID: z.string().uuid({ message: "اختر وحدة اختبار صالحة" }),
  score: z.number().finite().min(0, "يجب ألا تكون النتيجة سالبة"),
  maxScore: z.number().finite().min(0).optional(),
  notes: z.string().max(500, "يجب ألا تتجاوز الملاحظات 500 حرف").optional(),
});

const validateMaxScore = (result, context) => {
  if (result.maxScore !== undefined && result.score > result.maxScore) {
    context.addIssue({
      code: "custom",
      path: ["score"],
      message: `يجب ألا تتجاوز النتيجة ${result.maxScore}`,
    });
  }
};

export const ResultSchema = ResultFields.superRefine(validateMaxScore);
export const ResultUpdateSchema = ResultFields.extend({
  resultID: z.string().uuid({ message: "النتيجة غير صالحة" }),
}).superRefine(validateMaxScore);
