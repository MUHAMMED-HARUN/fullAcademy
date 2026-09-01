import TResult from "@/Generic/TResult";
import NeighborhoodMapper from "@/Mapper/Address/neighborhoodMapper";
import neighborhood_repositories from "@/repositories/Address/neighborhood_repositories";
import { NeighborhoodSchema } from "@/validations/Address/NeighborhoodSchema";
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
  async getById(param) {
    return await neighborhood_repositories.getById(param);
  }

  async create(data) {
    const validation = validate1(data, NeighborhoodSchema);

    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = NeighborhoodMapper.mapUpsertToApi(data);
    return await neighborhood_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, NeighborhoodSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = NeighborhoodMapper.mapUpsertToApi(data);
    return await neighborhood_repositories.update(payload);
  }
}

export default new NeighborhoodService();
