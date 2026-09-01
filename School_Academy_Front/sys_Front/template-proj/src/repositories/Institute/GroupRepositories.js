import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/Group";
class GroupRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewGroup`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateGroup`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل "
    );
    return response;
  }

  async getList() {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetGroupList`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة المجموعة"
    );
    return response;
  }

  async GetGroupsByTeacherID(param) {
    console.log("param", param);
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetGroupsByTeacherID`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب قائمة المجموعة"
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetGroupBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteGroup`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف بيانات "
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistGroupBy`,
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new GroupRepositories();
