import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/Payment";

class PaymentRepositories {
  async add(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/NewPayment`,
      enApiType.post,
      data,
      "حدث خطأ أثناء إضافة الدفعة"
    );
  }

  async update(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/UpdatePayment`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل الدفعة"
    );
  }

  async getList() {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetPaymentList`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب الدفعات"
    );
  }

  async getBy(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetPaymentBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب الدفعة"
    );
  }

  async Delete(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/DeletePayment`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف الدفعة"
    );
  }
}

export default new PaymentRepositories();
