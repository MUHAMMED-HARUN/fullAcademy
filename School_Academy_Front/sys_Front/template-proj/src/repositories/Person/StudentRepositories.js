import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Student";
class StudentRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewStudent`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateStudent`,
      enApiType.put,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async getList(filter) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentList`,
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب قائمة "
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteStudent`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف بيانات "
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistStudentBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new StudentRepositories();
