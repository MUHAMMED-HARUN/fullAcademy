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

  async reverse(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/ReversePayment`,
      enApiType.post,
      data,
      "حدث خطأ أثناء عكس الدفعة"
    );
  }

  async getListFilter(params) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetPaymentListFilter`,
      enApiType.get,
      { params },
      "خطأ في الخادم أثناء جلب الدفعات"
    );
  }

  async getPaymentsByInvoice(studentInvoiceID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetPaymentsByInvoice/${encodeURIComponent(studentInvoiceID)}`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب دفعات الفاتورة"
    );
  }

  async getBy(params) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetPaymentBy`,
      enApiType.get,
      { params },
      "خطأ في الخادم أثناء جلب الدفعة"
    );
  }
}

export default new PaymentRepositories();
