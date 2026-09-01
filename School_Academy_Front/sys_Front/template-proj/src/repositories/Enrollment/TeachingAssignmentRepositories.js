import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/TeachingAssignment";
class TeachingAssignmentRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewTeachingAssignment`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة طلب التسجيل"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateTeachingAssignment`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل طلب التسجيل"
    );
    return response;
  }

  async getList(queryParams) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetTeachingAssignmentList`,
      enApiType.get,
      { params: queryParams },
      "خطأ في الخادم أثناء جلب قائمة التسجيل"
    );
    return response;
  }
  async GetTeacherGroups(queryParams) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetTeacherGroups`,
      enApiType.get,
      { params: queryParams },
      "خطأ في الخادم أثناء جلب قائمة التسجيل"
    );
    return response;
  }
  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetTeachingAssignmentBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteTeachingAssignment`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف التسجيل"
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistTeachingAssignmentBy`,
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }
}

export default new TeachingAssignmentRepositories();
