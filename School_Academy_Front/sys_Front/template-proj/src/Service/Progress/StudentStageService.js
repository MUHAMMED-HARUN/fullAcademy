import TResult from "@/Generic/TResult";
import StudentStageMapper from "@/Mapper/Progress/StudentStageMapper";
import StudentStage_repositories from "@/repositories/Progress/StudentStageRepositories";
import {
  StudentStageSchema,
  StudentStageUpdateSchema,
} from "@/validations/Progress/StudentStageSchema";
import { validate1 } from "@/validations/validator";

class StudentStageService {
  async getAll(params = {}) {
    const response = await StudentStage_repositories.getList(params);
    if (!response.success) return new TResult(null, false, response.errors);
    const paging = response.data || {};
    return new TResult(
      {
        items: StudentStageMapper.mapListApiToStudentStage(
          paging.Data ?? paging.data ?? []
        ),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
  }

  async getBy(command) {
    const rees = await StudentStage_repositories.getBy(command);
    if (rees.success) {
      const dto = StudentStageMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, StudentStageSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentStageMapper.mapCreateToApi(data);
    return await StudentStage_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, StudentStageUpdateSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentStageMapper.mapUpdateToApi(data);
    return await StudentStage_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await StudentStage_repositories.Delete({
        StudentStageID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new StudentStageService();
