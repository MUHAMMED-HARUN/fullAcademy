import z from "zod";

const scopeReferenceFields = {
  1: "programID",
  2: "instituteClassID",
  3: "stageID",
  4: "stageInfoID",
};

const dateRangeRule = (data, ctx) => {
  if (
    data.effectiveTo &&
    data.effectiveFrom &&
    data.effectiveTo < data.effectiveFrom
  ) {
    ctx.addIssue({
      code: "custom",
      path: ["effectiveTo"],
      message: "تاريخ النهاية يجب أن يكون بعد تاريخ البداية",
    });
  }
};

export const AcademicPlanSchema = z
  .object({
    academicPlanID: z.string().optional().nullable(),
    name: z
      .string()
      .min(1, { message: "اسم الخطة مطلوب" })
      .max(200, { message: "اسم الخطة يجب ألا يتجاوز 200 حرف" }),
    providerInstituteID: z.string().min(1, { message: "المؤسسة مطلوبة" }),
    scopeType: z.coerce
      .number({ message: "نوع النطاق مطلوب" })
      .int()
      .min(1, { message: "نوع النطاق مطلوب" })
      .max(4, { message: "نوع النطاق غير صالح" }),
    programID: z.string().optional().nullable(),
    instituteClassID: z.string().optional().nullable(),
    stageID: z.string().optional().nullable(),
    stageInfoID: z.string().optional().nullable(),
    effectiveFrom: z.string().min(1, { message: "تاريخ البداية مطلوب" }),
    effectiveTo: z.string().optional().nullable(),
    notes: z
      .string()
      .max(1000, { message: "الملاحظات يجب ألا تتجاوز 1000 حرف" })
      .optional()
      .nullable(),
  })
  .superRefine((data, ctx) => {
    const field = scopeReferenceFields[Number(data.scopeType)];
    if (field && !data[field]) {
      ctx.addIssue({
        code: "custom",
        path: [field],
        message: "المرجع المختار للنطاق مطلوب",
      });
    }
    dateRangeRule(data, ctx);
  });

export const AcademicPlanUpdateSchema = z
  .object({
    academicPlanID: z.string().min(1, { message: "معرّف الخطة مطلوب" }),
    name: z
      .string()
      .min(1, { message: "اسم الخطة مطلوب" })
      .max(200, { message: "اسم الخطة يجب ألا يتجاوز 200 حرف" }),
    effectiveFrom: z.string().min(1, { message: "تاريخ البداية مطلوب" }),
    effectiveTo: z.string().optional().nullable(),
    notes: z
      .string()
      .max(1000, { message: "الملاحظات يجب ألا تتجاوز 1000 حرف" })
      .optional()
      .nullable(),
  })
  .superRefine(dateRangeRule);
