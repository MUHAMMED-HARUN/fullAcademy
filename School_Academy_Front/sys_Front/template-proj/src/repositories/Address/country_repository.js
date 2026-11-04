import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

class CountryRepository {
  async addCountry(countryData) {
    const response = await clsUtil.FeachRequset(
      `/Country/NewCountry`,
      enApiType.post,
      countryData,
      "حدث خطأ أثناء إضافة الدولة"
    );
    return response;
  }

  async updateCountry(countryData) {
    const response = await clsUtil.FeachRequset(
      "/Country/UpdateCountry",
      enApiType.put,
      countryData,
      "حدث خطأ أثناء إضافة الدولة"
    );
    return response;
  }

  async getCountryList(filter) {
    const response = await clsUtil.FeachRequset(
      "/Country/GetCountryList",
      enApiType.get,
      { params: filter },
      "خطأ في الخادم أثناء جلب قائمة الدول"
    );
    return response;
  }

  async getCountryOptions() {
    return await clsUtil.FeachRequset(
      "/Country/GetCountryOptions",
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب خيارات الدول"
    );
  }

  async getCountryById(param, paramType = 1) {
    const response = await clsUtil.FeachRequset(
      "/Country/GetCountryByID",
      enApiType.get,
      { params: { Param: param, ParamType: paramType } },
      "خطأ في الخادم أثناء جلب بيانات الدولة"
    );
    return response;
  }
}

export default new CountryRepository();
