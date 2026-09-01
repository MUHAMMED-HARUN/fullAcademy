import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

class CountryRepository {
  async addCountry(countryData) {
    const response = await clsUtil.FeachRequset(
      `/Country/NewCountry`,
      enApiType.put,
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

  async getCountryList() {
    const response = await clsUtil.FeachRequset(
      "/Country/GetCountryList",
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب قائمة الدول"
    );
    return response;
  }

  async getCountryById(param) {
    const response = await clsUtil.FeachRequset(
      "`/Country/GetCountryByID",
      enApiType.get,
      param,
      "خطأ في الخادم أثناء جلب بيانات الدولة"
    );
    return response;
  }
}

export default new CountryRepository();
