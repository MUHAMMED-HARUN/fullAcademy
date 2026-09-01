import TResult from "@/Generic/TResult";
import AddressMapper from "@/Mapper/Address/addressMapper";
import address_repositories from "@/repositories/Address/address_repositories";
import { addressSchema } from "@/validations/Address/addressSchema";
import { validate1 } from "@/validations/validator";
class AddressService {
  async getAll() {
    const rees = await address_repositories.getList();
    var dto;
    if (rees.success) {
      dto = AddressMapper.mapListApiToAddress(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(param) {
    return await address_repositories.getBy(param);
  }

  async create(data) {
    const validation = validate1(data, addressSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AddressMapper.mapUpsertToApi(data);
    return await address_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, addressSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = AddressMapper.mapUpsertToApi(data);
    return await address_repositories.update(payload);
  }
}

export default new AddressService();
