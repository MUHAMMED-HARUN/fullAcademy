import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/AcademicPlan";

class AcademicPlanRepositories {
  async add(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/NewAcademicPlan`,
      enApiType.post,
      data
    );
  }

  async update(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/UpdateAcademicPlan`,
      enApiType.put,
      data
    );
  }

  async activate(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/ActivateAcademicPlan`,
      enApiType.put,
      data
    );
  }

  async getListFilter(params) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetAcademicPlanListFilter`,
      enApiType.get,
      { params }
    );
  }

  async getBy(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetAcademicPlanBy`,
      enApiType.get,
      { params: param }
    );
  }

  async getForUpsert(academicPlanID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/get-for-upsert/${academicPlanID}`,
      enApiType.get,
      null
    );
  }

  async Delete(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/DeleteAcademicPlan`,
      enApiType.delete,
      param
    );
  }
}

export default new AcademicPlanRepositories();
