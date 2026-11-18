import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Result";
class ResultRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewResult`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة طلب التسجيل"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateResult`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل طلب التسجيل"
    );
    return response;
  }

  async getList(params = {}) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetResultListFilter`,
      enApiType.get,
      { params },
      "خطأ في الخادم أثناء جلب قائمة التسجيل"
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetResultBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }

  async getForUpsert(resultID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetResultForUpsert`,
      enApiType.get,
      { params: { ResultID: resultID } },
      "تعذر تحميل النتيجة للتعديل"
    );
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteResult`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف التسجيل"
    );
    return response;
  }
}

export default new ResultRepositories();
