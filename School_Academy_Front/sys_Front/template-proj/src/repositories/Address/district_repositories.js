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
      "/District/GetDistrictOptions",
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة الحارة"
    );
    return response;
  }

  async getDistrictList(filter) {
    return await clsUtil.FeachRequset(
      "/District/GetDistrictList",
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب قائمة المناطق"
    );
  }

  async getDistrictsByCityId(cityID, filter = {}) {
    return await clsUtil.FeachRequset(
      `/District/GetDistrictListByCityID/${encodeURIComponent(cityID)}`,
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب مناطق المدينة"
    );
  }

  async getById(param, paramType = 1) {
    const response = await clsUtil.FeachRequset(
      `/District/GetDistrictByID/${encodeURIComponent(param)}`,
      enApiType.get,
      { params: { paramType } },
      "خطأ في الخادم أثناء جلب بيانات الحارة"
    );
    return response;
  }
}

export default new districtRepositories();
