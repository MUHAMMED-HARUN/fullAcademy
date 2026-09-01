import TResult from "@/Generic/TResult";
import ResultMapper from "@/Mapper/Assigment/ResultMapper";
import ResultRepositories from "@/repositories/Assigment/ResultRepositories";
import { ResultSchema } from "@/validations/Assignment/ResultSchema";
import { validate1 } from "@/validations/validator";

class ResultService {
  async getAll() {
    const rees = await ResultRepositories.getList();
    var dto;
    if (rees.success) {
      dto = ResultMapper.mapListApiToResult(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const rees = await ResultRepositories.getBy(command);
    var dto;
    if (rees.success) {
      dto = ResultMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, ResultSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = ResultMapper.mapUpsertToApi(data);

    return await ResultRepositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, ResultSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = ResultMapper.mapUpsertToApi(data);
    return await ResultRepositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await ResultRepositories.Delete({
        ResultID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new ResultService();
