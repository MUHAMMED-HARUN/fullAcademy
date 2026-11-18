import TResult from "@/Generic/TResult";
import StudentAcademicEnrollmentMapper from "@/Mapper/Enrollment/StudentAcademicEnrollmentMapper";
import StudentAcademicEnrollmentRepositories from "@/repositories/Enrollment/StudentAcademicEnrollmentRepositories";
import { StudentAcademicEnrollmentSchema } from "@/validations/Enrollment/StudentAcademicEnrollmentSchema";
import { validate1 } from "@/validations/validator";

const safeText = (value) =>
  String(value || "")
    .replace(/[,*|=<>^$()\\;]/g, " ")
    .trim();

class StudentAcademicEnrollmentService {
  async getListFilter(params = {}) {
    const response = await StudentAcademicEnrollmentRepositories.getListFilter(
      params
    );
    if (!response.success) return new TResult(null, false, response.errors);
    const paging = response.data || {};
    const rawItems = paging.Data ?? paging.data ?? [];
    return new TResult(
      {
        items: rawItems.map((item) =>
          StudentAcademicEnrollmentMapper.mapLookupFromApi(item)
        ),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
  }

  async getForUpsert(id) {
    const response = await StudentAcademicEnrollmentRepositories.getForUpsert(
      id
    );
    if (!response.success) return new TResult(null, false, response.errors);
    return new TResult(
      StudentAcademicEnrollmentMapper.mapFromApi(response.data),
      true,
      response.errors
    );
  }

  async enroll(data) {
    const validation = validate1(data, StudentAcademicEnrollmentSchema);
    if (!validation.isValid) return new TResult(null, false, validation.errors);
    return await StudentAcademicEnrollmentRepositories.enrollStudentAcademicPlan(
      StudentAcademicEnrollmentMapper.mapCreateToApi(data)
    );
  }

  buildLookupFilter(type, value) {
    const text = safeText(value);
    if (!text) return "";
    if (type === "student") return `StudentName=*${text}`;
    return `AcademicPlanName=*${text}`;
  }
}

export default new StudentAcademicEnrollmentService();
