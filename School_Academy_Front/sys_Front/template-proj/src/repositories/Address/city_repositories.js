import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

class CityRepository {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      "/City/NewCity",
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة المدينة"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      "/City/UpdateCity",
      enApiType.put,
      data,
      "حدث خطأ أثناء إضافة المدينة"
    );
    return response;
  }

  async getList() {
    const response = await clsUtil.FeachRequset(
      "/City/GetCityList",
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة المدينة"
    );
    return response;
  }

  async getById(param) {
    const response = await clsUtil.FeachRequset(
      "`/City/GetCityByID",
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات المدينة"
    );
    return response;
  }
}

export default new CityRepository();
