import TResult from "@/Generic/TResult";
import StudentStageMapper from "@/Mapper/Progress/StudentStageMapper";
import StudentStage_repositories from "@/repositories/Progress/StudentStageRepositories";
import { StudentStageSchema } from "@/validations/Progress/StudentStageSchema";
import { validate1 } from "@/validations/validator";

class StudentStageService {
  async getAll() {
    const rees = await StudentStage_repositories.getList();
    var dto;
    if (rees.success) {
      console.log(rees);
      dto = StudentStageMapper.mapListApiToStudentStage(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const rees = await StudentStage_repositories.getBy(command);
    var dto;
    console.log(rees);
    if (rees.success) {
      dto = StudentStageMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, StudentStageSchema);
    if (!validation.isValid) {
      console.log("valid", validation);
      console.log("valid data", data);
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentStageMapper.mapUpsertToApi(data);
    return await StudentStage_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, StudentStageSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentStageMapper.mapUpsertToApi(data);
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
