import z from "zod";
import { GuidSchema } from "@/validations/Person/guidSchema";

const addressFields = {
  countryID: GuidSchema,
  cityID: GuidSchema,
  districtID: GuidSchema,
  neighborhoodID: GuidSchema,
};

export const NewAddressSchema = z.object({
  id: z.literal("").optional(),
  ...addressFields,
  notes: z
    .string()
    .trim()
    .min(1, "ملاحظات العنوان مطلوبة")
    .max(100, "لا يمكن ان تكون الملاحظات اكثر من 100 حرف"),
});

export const UpdateAddressSchema = z.object({
  id: GuidSchema,
  ...addressFields,
  notes: z
    .string()
    .trim()
    .max(100, "لا يمكن ان تكون الملاحظات اكثر من 100 حرف")
    .nullable()
    .optional(),
});

export const addressSchema = z.union([NewAddressSchema, UpdateAddressSchema]);

export const AddressLookupSchema = z.object({
  param: GuidSchema,
  paramType: z.union([z.literal(1), z.literal(2), z.literal(3)]),
});
