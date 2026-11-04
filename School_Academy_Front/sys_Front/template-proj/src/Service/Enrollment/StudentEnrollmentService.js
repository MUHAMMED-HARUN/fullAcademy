import TResult from "@/Generic/TResult";
import StudentEnrollmentMapper from "@/Mapper/Enrollment/StudentEnrollmentMapper";
import StudentEnrollment_repositories from "@/repositories/Enrollment/StudentEnrollmentRepositories";
import { StudentEnrollmentSchema } from "@/validations/Enrollment/StudentEnrollmentSchema";
import { validate1 } from "@/validations/validator";

class StudentEnrollmentService {
  async getPagedList(params) {
    const result = await StudentEnrollment_repositories.getPagedList(params);
    if (result.success) {
      const pagingObj = result.data || {};

      // استخراج القائمة والعدد الإجمالي من كائن Paging<T>
      const rawList = pagingObj.Data;
      const totalCount = pagingObj.Count ?? 0;

      // تحويل مصفوفة الطلاب عبر الـ Mapper
      const dto = StudentEnrollmentMapper.mapListApiToClass(rawList);

      // تمرير المابنج مع إجمالي العدد
      return new TResult(
        { items: dto, count: totalCount },
        result.success,
        result.errors
      );
    }
    return new TResult(null, false, result.errors);
  }

  async getBy(command) {
    return await StudentEnrollment_repositories.getBy(command);
  }

  async create(data) {
    const validation = validate1(data, StudentEnrollmentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentEnrollmentMapper.mapUpsertToApi(data);
    return await StudentEnrollment_repositories.add(payload);
  }

  async enrollByRange(data) {
    return await StudentEnrollment_repositories.enrollByRange(data);
  }

  async update(data) {
    const validation = validate1(data, StudentEnrollmentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentEnrollmentMapper.mapUpsertToApi(data);
    return await StudentEnrollment_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await StudentEnrollment_repositories.Delete({
        EnrollmentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new StudentEnrollmentService();
