import TResult from "@/Generic/TResult";
import AddressMapper from "@/Mapper/Address/addressMapper";
import address_repositories from "@/repositories/Address/address_repositories";
import {
  AddressLookupSchema,
  NewAddressSchema,
  UpdateAddressSchema,
} from "@/validations/Address/addressSchema";
import { validate1 } from "@/validations/validator";
class AddressService {
  async getBy(criteria) {
    const source =
      typeof criteria === "string" ? { param: criteria } : criteria || {};
    const validation = validate1(
      {
        param: source.Param ?? source.param,
        paramType: Number(source.ParamType ?? source.paramType ?? 1),
      },
      AddressLookupSchema
    );
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const result = await address_repositories.getBy({
      Param: source.Param ?? source.param,
      ParamType: Number(source.ParamType ?? source.paramType ?? 1),
    });
    if (!result.success) return result;

    return new TResult(
      AddressMapper.MapFromApiDTO(result.data),
      result.success,
      result.errors
    );
  }

  async create(data) {
    const validation = validate1(data, NewAddressSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AddressMapper.mapUpsertToApi(data);
    return await address_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, UpdateAddressSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = AddressMapper.mapUpsertToApi(data);
    return await address_repositories.update(payload);
  }
}

export default new AddressService();
