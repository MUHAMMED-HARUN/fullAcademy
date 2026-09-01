import TResult from "@/Generic/TResult";
import DailyEvaluationMapper from "@/Mapper/Progress/DailyEvaluationMapper";
import daily_evaluation_repositories from "@/repositories/Progress/DailyEvaluationRepositories";
import { DailyEvaluationSchema } from "@/validations/Progress/DailyEvaluationSchema";
import { validate1 } from "@/validations/validator";

class DailyEvaluationService {
  async getAll(filter) {
    const rees = await daily_evaluation_repositories.getPagedList(filter);
    if (rees.success) {
      const pagingObj = rees.data || {};

      // استخراج القائمة والعدد الإجمالي من كائن Paging<T>
      const rawList = pagingObj.Data;
      const totalCount = pagingObj.Count ?? 0;

      // تحويل مصفوفة التقييمات عبر الـ Mapper
      const dto = DailyEvaluationMapper.mapListApiToDailyEvaluation(rawList);

      // تمرير المابنج مع إجمالي العدد
      return new TResult(
        { items: dto, count: totalCount },
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }
  async GetlastDailyEvaluation(StudentEnrollmentID, StageInfoID) {
    const rees = await daily_evaluation_repositories.GetlastDailyEvaluation(
      StudentEnrollmentID,
      StageInfoID
    );
    var dto;
    if (rees.success) {
      dto = DailyEvaluationMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getBy(command) {
    var res = await daily_evaluation_repositories.getBy(command);
    var dto;
    if (res.success) {
      dto = DailyEvaluationMapper.MapFromApiDTO(res.data);
      return new TResult(dto, res.success, res.errors);
    }
    return new TResult(null, false, res.errors);
  }

  async create(data) {
    const validation = validate1(data, DailyEvaluationSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = DailyEvaluationMapper.mapUpsertToApi(data);
    return await daily_evaluation_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, DailyEvaluationSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = DailyEvaluationMapper.mapUpsertToApi(data);
    return await daily_evaluation_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await daily_evaluation_repositories.Delete({
        DailyEvaluationID: data,
      });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new DailyEvaluationService();
