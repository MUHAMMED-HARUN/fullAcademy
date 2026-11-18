import z from "zod";
export const StageInfoSchema = z
  .object({
    stageInfoID: z.union([z.string().uuid(), z.literal("")]).optional(),
    from: z.number().finite().min(0, "يجب أن تكون البداية صفراً أو أكبر"),
    to: z.number().finite().min(0, "يجب أن تكون النهاية صفراً أو أكبر"),
    stageID: z.string().uuid({ message: "اختر مرحلة صالحة" }),
    matterID: z.string().uuid({ message: "اختر مادة صالحة" }),
    uniteType: z.number().int().min(0).max(1),
    progressType: z.number().int().min(0).max(2),
  })
  .refine((data) => data.to >= data.from, {
    path: ["to"],
    message: "يجب أن تكون النهاية أكبر من البداية أو مساوية لها",
  });
