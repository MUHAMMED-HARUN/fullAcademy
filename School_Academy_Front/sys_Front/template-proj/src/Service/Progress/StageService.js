import TResult from "@/Generic/TResult";
import StageMapper from "@/Mapper/Progress/StageMapper";
import Stage_repositories from "@/repositories/Progress/StageRepositories";
import { StageSchema } from "@/validations/Progress/StageSchema";
import { validate1 } from "@/validations/validator";

class StageService {
  async getAll() {
    const rees = await Stage_repositories.getList();
    var dto;
    if (rees.success) {
      dto = StageMapper.mapListApiToStages(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const rees = await Stage_repositories.getBy(command);
    var dto;
    console.log(rees);
    if (rees.success) {
      dto = StageMapper.MapFromApiDTO(rees.data);
      console.log("dto", dto);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, StageSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StageMapper.mapUpsertToApi(data);
    return await Stage_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, StageSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StageMapper.mapUpsertToApi(data);
    return await Stage_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await Stage_repositories.Delete({
        EnrollmentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new StageService();
