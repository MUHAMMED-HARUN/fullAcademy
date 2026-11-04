import TResult from "@/Generic/TResult";
import districtMapper from "@/Mapper/Address/districtMapper";
import district_repositories from "@/repositories/Address/district_repositories";
import {
  DistrictLookupSchema,
  NewDistrictSchema,
  UpdateDistrictSchema,
} from "@/validations/Address/DistrictSchema";
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

  async getPage(filter) {
    const result = await district_repositories.getDistrictList(filter);
    if (!result.success) return result;

    return new TResult(
      districtMapper.mapPagingApiToDistrict(result.data),
      result.success,
      result.errors
    );
  }

  async getById(param, paramType = 1) {
    const result = await district_repositories.getById(param, paramType);
    if (!result.success) return result;

    return new TResult(
      districtMapper.MapFromApiDTO(result.data),
      result.success,
      result.errors
    );
  }

  async getByCityId(cityID, filter) {
    const result = await district_repositories.getDistrictsByCityId(
      cityID,
      filter
    );
    if (!result.success) return result;

    return new TResult(
      districtMapper.mapPagingApiToDistrict(result.data),
      result.success,
      result.errors
    );
  }

  async searchDistricts(criteria) {
    const validation = validate1(criteria, DistrictLookupSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const value = criteria.value?.trim() || "";
    return await this.getByCityId(criteria.cityID, {
      Page: 1,
      PageSize: 5,
      Filter: value ? `DistrictName=*${value}` : "",
    });
  }

  async create(data) {
    const validation = validate1(data, NewDistrictSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = districtMapper.mapUpsertToApi(data);
    return await district_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, UpdateDistrictSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = districtMapper.mapUpsertToApi(data);
    return await district_repositories.update(payload);
  }
}

export default new DistrictService();
