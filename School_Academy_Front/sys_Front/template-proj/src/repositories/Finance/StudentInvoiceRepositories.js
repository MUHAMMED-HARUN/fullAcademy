import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/StudentInvoice";

class StudentInvoiceRepositories {
  async update(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/UpdateStudentInvoice`,
      enApiType.put,
      data,
      "حدث خطأ أثناء تعديل فاتورة الطالب"
    );
  }

  async getListFilter(params) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentInvoiceListFilter`,
      enApiType.get,
      { params },
      "خطأ في الخادم أثناء جلب فواتير الطلاب"
    );
  }

  async generateForEnrollment(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GenerateInvoicesForEnrollment`,
      enApiType.post,
      data,
      "خطأ في الخادم أثناء إصدار الفاتورة"
    );
  }

  async getByAcademicEnrollment(studentAcademicEnrollmentID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetByAcademicEnrollment`,
      enApiType.get,
      { params: { StudentAcademicEnrollmentID: studentAcademicEnrollmentID } },
      "خطأ في الخادم أثناء جلب فواتير التسجيل"
    );
  }

  async getFinanceSummary(studentID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentFinanceSummary`,
      enApiType.get,
      { params: studentID ? { StudentID: studentID } : {} },
      "خطأ في الخادم أثناء جلب الملخص المالي"
    );
  }

  async getForUpsert(studentInvoiceID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/get-for-upsert/${encodeURIComponent(studentInvoiceID)}`,
      enApiType.get,
      null,
      "خطأ في الخادم أثناء جلب تفاصيل الفاتورة"
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
