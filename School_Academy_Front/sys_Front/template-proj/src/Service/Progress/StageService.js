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
    if (rees.success) {
      dto = StageMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getAllFilter(params = {}) {
    const response = await Stage_repositories.getListFilter(params);
    if (!response.success) return new TResult(null, false, response.errors);

    const paging = response.data || {};
    const items = paging.Data ?? paging.data ?? [];
    return new TResult(
      {
        items: StageMapper.mapListApiToStageLookups(items),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
  }

  async searchPreviousStages({ term, classID, excludeStageID }) {
    const safeTerm = String(term ?? "")
      .trim()
      .replace(/[,*|^$!=()/]/g, " ")
      .trim();
    if (!safeTerm || !classID) return new TResult([], true, null);

    const result = await this.getAllFilter({
      Page: 1,
      PageSize: 10,
      Filter: `StageName=*${safeTerm}`,
      ClassID: classID,
      ExcludeStageID: excludeStageID || "",
    });
    if (!result.success) return result;
    return new TResult(result.data?.items || [], true, result.errors);
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
      return await Stage_repositories.Delete(data);
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new StageService();
