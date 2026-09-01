import TResult from "@/Generic/TResult";
import StageInfoMapper from "@/Mapper/Progress/StageInfoMapper";
import StageInfo_repositories from "@/repositories/Progress/StageInfoRepositories";
import { StageInfoSchema } from "@/validations/Progress/StageInfoSchema";
import { validate1 } from "@/validations/validator";

class StageInfoService {
  async getAll() {
    const rees = await StageInfo_repositories.getList();
    var dto;
    if (rees.success) {
      dto = StageInfoMapper.mapListApiToStagesInfo(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getStudentStageInfo(enrollmentID) {
    const rees = await StageInfo_repositories.getStudentStageInfo({
      enrollmentID,
    });
    var dto;
    if (rees.success) {
      dto = StageInfoMapper.mapListApiToStagesInfo(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getBy(command) {
    const rees = await StageInfo_repositories.getBy(command);
    var dto;
    if (rees.success) {
      dto = StageInfoMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, StageInfoSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StageInfoMapper.mapUpsertToApi(data);
    return await StageInfo_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, StageInfoSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StageInfoMapper.mapUpsertToApi(data);
    return await StageInfo_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await StageInfo_repositories.Delete({
        EnrollmentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new StageInfoService();
