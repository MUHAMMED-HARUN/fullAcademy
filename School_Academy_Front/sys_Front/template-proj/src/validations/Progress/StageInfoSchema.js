import z from "zod";
export const StageInfoSchema = z.object({
  stageInfoID: z.string().optional(),
  from: z.number("  مطلوب").min(0, "  يجب أن يكون اكبر من 0"),
  to: z.number("  مطلوب").min(0, "  يجب أن يكون اكبر من 0"),
  stageID: z.string().optional("مطلوب"),
  matterID: z.string().optional("مطلوب"),
  uniteType: z.number("  مطلوب").min(0, "  يجب أن يكون حرفين على الأقل"),
  progressType: z.number("  مطلوب").min(0, "  يجب أن يكون حرفين على الأقل"),
});
