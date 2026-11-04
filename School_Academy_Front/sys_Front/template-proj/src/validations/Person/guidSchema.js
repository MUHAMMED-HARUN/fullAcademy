import z from "zod";

const guidPattern = /^[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}$/i;

export const GuidSchema = z
  .string()
  .regex(guidPattern, { message: "معرّف غير صالح" })
  .refine(
    (value) => value.toLowerCase() !== "00000000-0000-0000-0000-000000000000",
    { message: "يجب اختيار معرّف صالح" }
  );

export const RequiredTextSchema = z
  .string()
  .min(1, { message: "مطلوب" })
  .refine((value) => value.trim().length > 0, { message: "مطلوب" });

const localDateOnly = (date) => {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};

const todayDateOnly = () => localDateOnly(new Date());

const isValidDateOnly = (value) => {
  let dateOnly;

  if (value instanceof Date) {
    if (Number.isNaN(value.getTime())) return false;
    dateOnly = localDateOnly(value);
  } else if (typeof value === "string") {
    if (!/^\d{4}-\d{2}-\d{2}$/.test(value)) return false;
    const parsed = new Date(`${value}T00:00:00.000Z`);
    if (
      Number.isNaN(parsed.getTime()) ||
      parsed.toISOString().slice(0, 10) !== value
    ) {
      return false;
    }
    dateOnly = value;
  } else {
    return false;
  }

  return dateOnly !== "0001-01-01" && dateOnly <= todayDateOnly();
};

export const BirthDateSchema = z
  .union([z.string(), z.date()])
  .refine(isValidDateOnly, {
    message: "تاريخ الولادة مطلوب ويجب ألا يكون في المستقبل",
  });

export const makeLookupSchema = (allowedTypes) =>
  z.object({
    Param: RequiredTextSchema,
    ParamType: z.union(allowedTypes.map((value) => z.literal(value))),
  });
