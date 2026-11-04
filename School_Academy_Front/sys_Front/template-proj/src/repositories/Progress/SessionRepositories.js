import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Session";
class SessionRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewSession`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة طلب التسجيل"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateSession`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل طلب التسجيل"
    );
    return response;
  }

  async getListFilter(params) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetSessionListFilter`,
      enApiType.get,
      { params },
      "خطأ في الخادم أثناء جلب قائمة المحتوى"
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetSessionBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteSession`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف التسجيل"
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistSessionBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء فحص المحتوى"
    );
    return response;
  }
}

export default new SessionRepositories();
