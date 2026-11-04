import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

class neighborhoodRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      "/Neighborhood/NewNeighborhood",
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      "/Neighborhood/UpdateNeighborhood",
      enApiType.put,
      data,
      "حدث خطأ أثناء إضافة "
    );
    return response;
  }

  async getList() {
    const response = await clsUtil.FeachRequset(
      "/Neighborhood/GetNeighborhoodOptions",
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة "
    );
    return response;
  }

  async getNeighborhoodList(filter) {
    return await clsUtil.FeachRequset(
      "/Neighborhood/GetNeighborhoodList",
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب قائمة الشوارع"
    );
  }

  async getNeighborhoodsByDistrictId(districtID, filter = {}) {
    return await clsUtil.FeachRequset(
      `/Neighborhood/GetNeighborhoodListByDistrictID/${encodeURIComponent(
        districtID
      )}`,
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب شوارع المنطقة"
    );
  }

  async getById(param, paramType = 1) {
    const response = await clsUtil.FeachRequset(
      `/Neighborhood/GetNeighborhoodByID/${encodeURIComponent(param)}`,
      enApiType.get,
      { params: { paramType } },
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new neighborhoodRepositories();
