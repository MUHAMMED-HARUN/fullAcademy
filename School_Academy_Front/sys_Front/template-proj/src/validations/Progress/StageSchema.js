import z from "zod";
export const StageSchema = z.object({
  stageID: z.string().optional(),
  name: z.string().nonempty({ message: "مطلوب" }),
  note: z.string().nonempty({ message: "مطلوب" }),
  programID: z.string().optional({ message: "مطلوب" }),
  prevStageID: z.string().optional({ message: "مطلوب" }),
});
