import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Teacher";

class TeacherRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewTeacher`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateTeacher`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل "
    );
    return response;
  }

  async getList(filter) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetTeacherList`,
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب قائمة "
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetTeacherBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteTeacher`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف بيانات "
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistTeacherBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new TeacherRepositories();
