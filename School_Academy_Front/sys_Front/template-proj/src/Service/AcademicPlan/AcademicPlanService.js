import TResult from "@/Generic/TResult";
import AcademicPlanMapper from "@/Mapper/AcademicPlan/AcademicPlanMapper";
import AcademicPlanRepositories from "@/repositories/AcademicPlan/AcademicPlanRepositories";
import {
  AcademicPlanSchema,
  AcademicPlanUpdateSchema,
} from "@/validations/AcademicPlan/AcademicPlanSchema";
import { validate1 } from "@/validations/validator";

class AcademicPlanService {
  async getAllFilter(params = {}) {
    const response = await AcademicPlanRepositories.getListFilter(params);
    if (!response.success) return new TResult(null, false, response.errors);

    const paging = response.data || {};
    return new TResult(
      {
        items: AcademicPlanMapper.mapLookupListToClass(
          paging.Data ?? paging.data ?? []
        ),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
  }

  async getForUpsert(academicPlanID) {
    const result = await AcademicPlanRepositories.getForUpsert(academicPlanID);
    if (!result.success) return new TResult(null, false, result.errors);
    return new TResult(
      AcademicPlanMapper.MapFromApiDTO(result.data),
      true,
      result.errors
    );
  }

  async create(data) {
    const validation = validate1(data, AcademicPlanSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const payload = AcademicPlanMapper.mapCreateToApi(data);
    return await AcademicPlanRepositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, AcademicPlanUpdateSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const payload = AcademicPlanMapper.mapUpdateToApi(data);
    return await AcademicPlanRepositories.update(payload);
  }

  async activate(academicPlanID) {
    if (!academicPlanID) {
      return new TResult(null, false, ["معرّف الخطة مطلوب"]);
    }
    return await AcademicPlanRepositories.activate({
      AcademicPlanID: academicPlanID,
    });
  }

  async delete(academicPlanID) {
    if (!academicPlanID) {
      return new TResult(null, false, ["معرّف الخطة مطلوب"]);
    }
    return await AcademicPlanRepositories.Delete({
      AcademicPlanID: academicPlanID,
    });
  }
}

export default new AcademicPlanService();
