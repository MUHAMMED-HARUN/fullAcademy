import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/StudentEnrollment";
class StudentEnrollmentRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewStudentEnrollment`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة طلب التسجيل"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateStudentEnrollment`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل طلب التسجيل"
    );
    return response;
  }

  async getPagedList(params) {
    // params: { page: 1, pageSize: 10, filter: 'EnrollmentID=xxx', orderBy: 'Date desc' }
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentEnrollmentList`,
      enApiType.get,
      { params: params },
      "خطأ في الخادم أثناء جلب قائمة التسجيل"
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentEnrollmentBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteStudentEnrollment`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف التسجيل"
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistStudentEnrollmentBy`,
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }
}

export default new StudentEnrollmentRepositories();
