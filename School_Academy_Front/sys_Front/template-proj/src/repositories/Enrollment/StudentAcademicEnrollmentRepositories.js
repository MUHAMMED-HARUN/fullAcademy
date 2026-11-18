import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/StudentAcademicEnrollment";

class StudentAcademicEnrollmentRepositories {
  async enrollStudentAcademicPlan(data) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/EnrollStudentAcademicPlan`,
      enApiType.post,
      data
    );
  }

  async getListFilter(params) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentAcademicEnrollmentListFilter`,
      enApiType.get,
      { params }
    );
  }

  async getForUpsert(studentAcademicEnrollmentID) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/get-for-upsert/${studentAcademicEnrollmentID}`,
      enApiType.get,
      null
    );
  }
}

export default new StudentAcademicEnrollmentRepositories();
