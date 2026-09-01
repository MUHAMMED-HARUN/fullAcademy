import z from "zod";

export const SessionSchema = z.object({
  sessionID: z.string().optional(),
  name: z.string().nonempty({ message: "مطلوب" }),
  type: z.string().nonempty({ message: "مطلوب" }),
  note: z.string().nonempty({ message: "مطلوب" }),
  parentSessionID: z.string().optional({ message: "مطلوب" }),
});
