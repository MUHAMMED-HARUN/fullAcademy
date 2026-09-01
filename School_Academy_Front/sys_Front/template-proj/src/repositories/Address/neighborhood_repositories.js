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
      "/Neighborhood/GetNeighborhoodList",
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة "
    );
    return response;
  }

  async getById(param) {
    const response = await clsUtil.FeachRequset(
      "/Neighborhood/GetNeighborhoodByID",
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات "
    );
    return response;
  }
}

export default new neighborhoodRepositories();
