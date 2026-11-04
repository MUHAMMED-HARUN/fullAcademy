import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/StudentInvoice";

class StudentInvoiceRepositories {
  async add(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/NewStudentInvoice`,
      enApiType.post,
      data,
      "حدث خطأ أثناء تسجيل الطالب في الخطة"
    );
  }

  async update(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/UpdateStudentInvoice`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل فاتورة الطالب"
    );
  }

  async getList() {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentInvoiceList`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب فواتير الطلاب"
    );
  }

  async getBy(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentInvoiceBy`,
      enApiType.get,
      { params: param },
      "خطأ في الخادم أثناء جلب فاتورة الطالب"
    );
  }

  async Delete(param) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/DeleteStudentInvoice`,
      enApiType.delete,
      param,
      "خطأ في الخادم أثناء حذف فاتورة الطالب"
    );
  }
}

export default new StudentInvoiceRepositories();
