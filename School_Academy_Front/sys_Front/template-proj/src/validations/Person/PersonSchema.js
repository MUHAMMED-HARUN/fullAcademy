import z from "zod";
import {
  BirthDateSchema,
  GuidSchema,
  makeLookupSchema,
  RequiredTextSchema,
} from "@/validations/Person/guidSchema";

const personNameSchema = (message) =>
  z
    .string()
    .min(1, { message: "مطلوب" })
    .max(60, { message })
    .refine((value) => value.trim().length > 0, { message: "مطلوب" });

const personFields = {
  firstName: personNameSchema("الاسم الأول طويل جداً"),
  fatherName: personNameSchema("اسم الأب طويل جداً"),
  lastName: personNameSchema("الكنية طويلة جداً"),
  motherName: personNameSchema("اسم الأم طويل جداً"),
  motherLastName: personNameSchema("كنية الأم طويلة جداً"),
  nationalNumber: RequiredTextSchema,
  gender: z.boolean(),
  birthDate: BirthDateSchema,
  placeOfBirthID: GuidSchema,
};

const personAddressFields = {
  countryID: GuidSchema,
  cityID: GuidSchema,
  districtID: GuidSchema,
  neighborhoodID: GuidSchema,
};

const personBaseSchema = z.object({
  id: z.string().optional(),
  ...personFields,
  address: z.object({
    id: z.string().optional(),
    ...personAddressFields,
    notes: z
      .string()
      .max(100, { message: "الملاحظات طويلة جداً" })
      .nullable()
      .optional(),
  }),
});

const addGuidIssue = (context, path, value, message) => {
  if (!GuidSchema.safeParse(value).success) {
    context.addIssue({ code: "custom", path, message });
  }
};

const validateCreate = (person, context) => {
  if (person.id && person.id !== "") {
    context.addIssue({
      code: "custom",
      path: ["id"],
      message: "معرّف الشخص غير متوقع عند الإنشاء",
    });
  }

  if (!person.address.notes || !person.address.notes.trim()) {
    context.addIssue({
      code: "custom",
      path: ["address", "notes"],
      message: "الملاحظات مطلوبة عند إنشاء العنوان",
    });
  }
};

const validateUpdate = (person, context) => {
  addGuidIssue(
    context,
    ["id"],
    person.id,
    "Person ID must be a valid, non-empty GUID"
  );
  addGuidIssue(
    context,
    ["address", "id"],
    person.address.id,
    "Address ID must be a valid, non-empty GUID"
  );
};

export const NewPersonSchema = personBaseSchema.superRefine(validateCreate);
export const UpdatePersonSchema = personBaseSchema.superRefine(validateUpdate);

// Keep the existing service import valid while reporting operation-specific field errors.
export const PersonSchema = personBaseSchema.superRefine((person, context) => {
  if (person.id) validateUpdate(person, context);
  else validateCreate(person, context);
});

export const PersonLookupSchema = makeLookupSchema([1, 2]);
export const PersonExistenceSchema = PersonLookupSchema;
export const PersonForUpsertIdSchema = GuidSchema;
export const PersonSearchSchema = z.object({
  filterType: z.enum(["FirstName", "NationalNumber"]),
  value: z
    .string()
    .min(1, { message: "أدخل قيمة للبحث" })
    .max(100, { message: "قيمة البحث طويلة جداً" })
    .refine((value) => value.trim().length > 0, { message: "أدخل قيمة للبحث" })
    .refine((value) => !/[,|=*()<>!^$/]/.test(value), {
      message: "تحتوي قيمة البحث على رموز غير مسموحة",
    }),
});
