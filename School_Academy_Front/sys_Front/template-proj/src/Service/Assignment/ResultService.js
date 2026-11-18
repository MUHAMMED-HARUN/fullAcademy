import TResult from "@/Generic/TResult";
import ResultMapper from "@/Mapper/Assigment/ResultMapper";
import ResultRepositories from "@/repositories/Assigment/ResultRepositories";
import {
  ResultSchema,
  ResultUpdateSchema,
} from "@/validations/Assignment/ResultSchema";
import { validate1 } from "@/validations/validator";

class ResultService {
  async getAll(params = {}) {
    const response = await ResultRepositories.getList(params);
    if (!response.success) return new TResult(null, false, response.errors);
    const paging = response.data || {};
    return new TResult(
      {
        items: ResultMapper.mapListApiToResult(
          paging.Data ?? paging.data ?? []
        ),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
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

  async getForUpsert(resultID) {
    const response = await ResultRepositories.getForUpsert(resultID);
    return response.success
      ? new TResult(
          ResultMapper.MapFromApiDTO(response.data),
          true,
          response.errors
        )
      : new TResult(null, false, response.errors);
  }

  async create(data) {
    const validation = validate1(data, ResultSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = ResultMapper.mapCreateToApi(data);

    return await ResultRepositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, ResultUpdateSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = ResultMapper.mapUpdateToApi(data);
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
