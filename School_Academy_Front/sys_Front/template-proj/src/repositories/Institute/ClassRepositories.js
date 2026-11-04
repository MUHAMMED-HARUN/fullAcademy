import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Class";
class ClassRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewClass`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateClass`,
      enApiType.put,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async getList() {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetClassList`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة "
    );
    return response;
  }

  async getBy(param) {
    console.log("param", param);
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetClassBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteClass`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف بيانات "
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistClassBy`,
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new ClassRepositories();
