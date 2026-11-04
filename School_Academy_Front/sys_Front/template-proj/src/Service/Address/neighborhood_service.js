import TResult from "@/Generic/TResult";
import NeighborhoodMapper from "@/Mapper/Address/neighborhoodMapper";
import neighborhood_repositories from "@/repositories/Address/neighborhood_repositories";
import {
  NeighborhoodLookupSchema,
  NewNeighborhoodSchema,
  UpdateNeighborhoodSchema,
} from "@/validations/Address/NeighborhoodSchema";
import { validate1 } from "@/validations/validator";
class NeighborhoodService {
  async getAll() {
    const rees = await neighborhood_repositories.getList();
    var dto;
    if (rees.success) {
      dto = NeighborhoodMapper.mapListApiToNeighborhood(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getPage(filter) {
    const result = await neighborhood_repositories.getNeighborhoodList(filter);
    if (!result.success) return result;

    return new TResult(
      NeighborhoodMapper.mapPagingApiToNeighborhood(result.data),
      result.success,
      result.errors
    );
  }

  async getById(param, paramType = 1) {
    const result = await neighborhood_repositories.getById(param, paramType);
    if (!result.success) return result;

    return new TResult(
      NeighborhoodMapper.MapFromApiDTO(result.data),
      result.success,
      result.errors
    );
  }

  async getByDistrictId(districtID, filter) {
    const result = await neighborhood_repositories.getNeighborhoodsByDistrictId(
      districtID,
      filter
    );
    if (!result.success) return result;

    return new TResult(
      NeighborhoodMapper.mapPagingApiToNeighborhood(result.data),
      result.success,
      result.errors
    );
  }

  async searchNeighborhoods(criteria) {
    const validation = validate1(criteria, NeighborhoodLookupSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const value = criteria.value?.trim() || "";
    return await this.getByDistrictId(criteria.districtID, {
      Page: 1,
      PageSize: 5,
      Filter: value ? `NeighborhoodName=*${value}` : "",
    });
  }

  async create(data) {
    const validation = validate1(data, NewNeighborhoodSchema);

    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = NeighborhoodMapper.mapUpsertToApi(data);
    return await neighborhood_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, UpdateNeighborhoodSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = NeighborhoodMapper.mapUpsertToApi(data);
    return await neighborhood_repositories.update(payload);
  }
}

export default new NeighborhoodService();
