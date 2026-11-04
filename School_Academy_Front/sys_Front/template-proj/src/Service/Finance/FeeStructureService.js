import TResult from "@/Generic/TResult";
import FeeStructureMapper from "@/Mapper/Finance/FeeStructureMapper";
import FeeStructureRepositories from "@/repositories/Finance/FeeStructureRepositories";
import { FeeStructureSchema } from "@/validations/Finance/FeeStructureSchema";
import { validate1 } from "@/validations/validator";

class FeeStructureService {
  async getAll() {
    const result = await FeeStructureRepositories.getList();
    if (result.success) {
      const dto = FeeStructureMapper.mapListApiToClass(result.data);
      return new TResult(dto, result.success, result.errors);
    }
    return new TResult(null, false, result.errors);
  }

  async getBy(command) {
    const result = await FeeStructureRepositories.getBy(command);
    if (result.success) {
      return new TResult(
        FeeStructureMapper.MapFromApiDTO(result.data),
        result.success,
        result.errors
      );
    }
    return new TResult(null, false, result.errors);
  }

  async create(data) {
    const validation = validate1(data, FeeStructureSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = FeeStructureMapper.mapUpsertToApi(data);
    return await FeeStructureRepositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, FeeStructureSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = FeeStructureMapper.mapUpsertToApi(data);
    return await FeeStructureRepositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await FeeStructureRepositories.Delete({
        FeeStructureID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new FeeStructureService();
