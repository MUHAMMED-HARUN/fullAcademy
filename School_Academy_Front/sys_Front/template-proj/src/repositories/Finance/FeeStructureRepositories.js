import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/FeeStructure";

class FeeStructureRepositories {
  async add(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/NewFeeStructure`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة خطة الرسوم"
    );
  }

  async update(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/UpdateFeeStructure`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل خطة الرسوم"
    );
  }

  async getList() {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetFeeStructureList`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب خطط الرسوم"
    );
  }

  async getBy(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetFeeStructureBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب خطة الرسوم"
    );
  }

  async Delete(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/DeleteFeeStructure`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف خطة الرسوم"
    );
  }
}

export default new FeeStructureRepositories();
