import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/InstituteClass";
class InstituteClassRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewInstituteClass`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async addByRange(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewInstituteClassByRange`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateInstituteClass`,
      enApiType.put,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async getList() {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetInstituteClassList`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة "
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetInstituteClassBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteInstituteClass`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف بيانات "
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistInstituteClassBy`,
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new InstituteClassRepositories();
