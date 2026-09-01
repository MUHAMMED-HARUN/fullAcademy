import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

class districtRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      "/District/NewDistrict",
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة الحارة"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      "/District/UpdateDistrict",
      enApiType.put,
      data,
      "حدث خطأ أثناء إضافة الحارة"
    );
    return response;
  }

  async getList() {
    const response = await clsUtil.FeachRequset(
      "/District/GetDistrictList",
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة الحارة"
    );
    return response;
  }

  async getById(param) {
    const response = await clsUtil.FeachRequset(
      "/District/GetDistrictByID",
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات الحارة"
    );
    return response;
  }
}

export default new districtRepositories();
