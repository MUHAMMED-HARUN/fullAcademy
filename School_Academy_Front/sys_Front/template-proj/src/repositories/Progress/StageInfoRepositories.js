import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/StageInfo";
class StageInfoRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewStageInfo`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة طلب التسجيل"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateStageInfo`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل طلب التسجيل"
    );
    return response;
  }

  async getList() {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStageInfoList`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة التسجيل"
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStageInfoBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }
  async getStudentStageInfo(param) {
    console.log(param);
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentStageInfo`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteStageInfo`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف التسجيل"
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistStageInfoBy`,
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }
}

export default new StageInfoRepositories();
