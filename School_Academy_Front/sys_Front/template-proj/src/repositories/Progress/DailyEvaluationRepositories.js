import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/DailyEvaluation";

class DailyEvaluationRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewDailyEvaluation`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة التقييم اليومي"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateDailyEvaluation`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل التقييم اليومي"
    );
    return response;
  }

  // تحديث الجلب لدعم الفلترة والترقيم عبر Query Parameters
  async getPagedList(params) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetDailyEvaluationList`,
      enApiType.get,
      { params },
      "خطأ في الخادم أثناء جلب قائمة التقييمات اليومية"
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetDailyEvaluationBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التقييم"
    );
    return response;
  }

  async GetlastDailyEvaluation(StudentEnrollmentID, StageInfoID) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetlastDailyEvaluation`,
      enApiType.get,
      { params: { StudentEnrollmentID, StageInfoID } },
      "خطأ في الخادم أثناء جلب بيانات التقييم"
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteDailyEvaluation`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف التقييم"
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistDailyEvaluationBy`,
      enApiType.get,
      param,
      "خطأ في الخادم أثناء فحص وجود التقييم"
    );
    return response;
  }
}

export default new DailyEvaluationRepositories();
