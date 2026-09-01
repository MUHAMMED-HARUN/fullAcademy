import TResult from "@/Generic/TResult";
import districtMapper from "@/Mapper/Address/districtMapper";
import district_repositories from "@/repositories/Address/district_repositories";
import { DistrictSchema } from "@/validations/Address/DistrictSchema";
import { validate1 } from "@/validations/validator";
class DistrictService {
  async getAll() {
    const rees = await district_repositories.getList();
    var Districts;
    if (rees.success) {
      Districts = districtMapper.mapListApiToDistrict(rees.data);
      return new TResult(Districts, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getById(param) {
    return await district_repositories.getById(param);
  }

  async create(data) {
    const validation = validate1(data, DistrictSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = districtMapper.mapUpsertToApi(data);
    return await district_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, DistrictSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = districtMapper.mapUpsertToApi(data);
    return await district_repositories.update(payload);
  }
}

export default new DistrictService();
