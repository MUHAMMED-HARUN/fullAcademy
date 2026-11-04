import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Person";

class PersonRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewPerson`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdatePerson`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل "
    );
    return response;
  }

  async getList(filter) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetPersonList`,
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب قائمة "
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetPersonBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }

  async getForUpsert(personID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/get-for-upsert/${encodeURIComponent(personID)}`,
      enApiType.get,
      null,
      "تعذر جلب بيانات الشخص للتعديل"
    );
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeletePerson`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف بيانات "
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistPersonBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new PersonRepositories();
