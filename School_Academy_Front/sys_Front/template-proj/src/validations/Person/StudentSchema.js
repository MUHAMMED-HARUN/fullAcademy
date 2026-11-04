import z from "zod";
import { GuidSchema, makeLookupSchema } from "@/validations/Person/guidSchema";

const studentFields = {
  studentID: z.string().optional(),
  personID: GuidSchema,
};

export const NewStudentSchema = z.object({
  studentID: z.literal("").optional(),
  personID: GuidSchema,
});

export const UpdateStudentSchema = z.object({
  studentID: GuidSchema,
  personID: GuidSchema,
});

export const StudentSchema = z
  .object(studentFields)
  .superRefine((student, context) => {
    if (student.studentID && !GuidSchema.safeParse(student.studentID).success) {
      context.addIssue({
        code: "custom",
        path: ["studentID"],
        message: "Student ID must be a valid, non-empty GUID",
      });
    }
  });

export const StudentLookupSchema = makeLookupSchema([1, 2, 3]);
export const StudentExistenceSchema = StudentLookupSchema;
