import z from "zod";
export const TeacherSchema = z.object({
  id: z.string().optional(),
  personID: z.string().nonempty({ message: "  مطلوب" }),
});
