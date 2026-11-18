import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Assignment";
class AssignmentRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewAssignment`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة طلب التسجيل"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateAssignment`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل طلب التسجيل"
    );
    return response;
  }

  async getList(params = {}) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetAssignmentListFilter`,
      enApiType.get,
      { params },
      "خطأ في الخادم أثناء جلب قائمة التسجيل"
    );
    return response;
  }

  async getForUpsert(assignmentID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetAssignmentForUpsert`,
      enApiType.get,
      { params: { AssignmentID: assignmentID } },
      "تعذر تحميل بيانات الاختبار"
    );
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetAssignmentBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات التسجيل"
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteAssignment`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف التسجيل"
    );
    return response;
  }
}

export default new AssignmentRepositories();
