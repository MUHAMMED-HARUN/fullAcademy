import z from "zod";
export const StudentSchema = z.object({
  id: z.string().optional(),
  personID: z.string().nonempty({ message: "  مطلوب" }),
});
