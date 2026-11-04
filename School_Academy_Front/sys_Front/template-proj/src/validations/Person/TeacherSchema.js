import z from "zod";
import { GuidSchema, makeLookupSchema } from "@/validations/Person/guidSchema";

export const NewTeacherSchema = z.object({
  teacherID: z.literal("").optional(),
  personID: GuidSchema,
});

export const UpdateTeacherSchema = z.object({
  teacherID: GuidSchema,
  personID: GuidSchema,
});

export const TeacherSchema = z
  .object({
    teacherID: z.string().optional(),
    personID: GuidSchema,
  })
  .superRefine((teacher, context) => {
    if (teacher.teacherID && !GuidSchema.safeParse(teacher.teacherID).success) {
      context.addIssue({
        code: "custom",
        path: ["teacherID"],
        message: "Teacher ID must be a valid, non-empty GUID",
      });
    }
  });

export const TeacherLookupSchema = makeLookupSchema([1, 2, 3]);
export const TeacherExistenceSchema = TeacherLookupSchema;
