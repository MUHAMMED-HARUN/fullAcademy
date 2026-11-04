import z from "zod";

export const FeeStructureSchema = z.object({
  feeStructureID: z.string().optional().nullable(),
  name: z.string().nonempty({ message: "اسم الخطة مطلوب" }),
  feeType: z.coerce.number({ message: "نوع الرسوم مطلوب" }).min(1, {
    message: "نوع الرسوم مطلوب",
  }),
  amount: z.coerce.number({ message: "المبلغ مطلوب" }).min(0, {
    message: "المبلغ يجب أن يكون أكبر من أو يساوي 0",
  }),
  programID: z.string().optional().nullable(),
  classID: z.string().optional().nullable(),
  stageID: z.string().optional().nullable(),
  stageInfoID: z.string().optional().nullable(),
  notes: z.string().optional().nullable(),
});
