import z from "zod";

const dateSchema = z
  .string()
  .regex(/^\d{4}-\d{2}-\d{2}$/, "أدخل تاريخاً صالحاً");

export const StudentAcademicEnrollmentSchema = z
  .object({
    studentID: z.string().uuid({ message: "اختر طالباً صالحاً" }),
    academicPlanID: z.string().uuid({ message: "اختر خطة أكاديمية فعالة" }),
    enrollmentDate: dateSchema,
    validFrom: dateSchema,
    validTo: z.union([z.literal(""), dateSchema]).optional(),
  })
  .refine((data) => !data.validTo || data.validTo >= data.validFrom, {
    message: "تاريخ نهاية الصلاحية يجب ألا يسبق بدايتها",
    path: ["validTo"],
  })
  .refine(
    (data) =>
      data.enrollmentDate >= data.validFrom &&
      (!data.validTo || data.enrollmentDate <= data.validTo),
    {
      message: "تاريخ التسجيل يجب أن يقع ضمن فترة الصلاحية",
      path: ["enrollmentDate"],
    }
  );
