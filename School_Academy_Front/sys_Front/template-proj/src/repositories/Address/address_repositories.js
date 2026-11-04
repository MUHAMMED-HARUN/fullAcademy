import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

class addressRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      "/Address/NewAddress",
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      "/Address/UpdateAddress",
      enApiType.put,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      "/Address/GetAddressBy",
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new addressRepositories();
