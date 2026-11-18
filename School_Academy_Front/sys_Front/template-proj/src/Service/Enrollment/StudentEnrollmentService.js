import TResult from "@/Generic/TResult";
import StudentEnrollmentMapper from "@/Mapper/Enrollment/StudentEnrollmentMapper";
import StudentEnrollment_repositories from "@/repositories/Enrollment/StudentEnrollmentRepositories";
import {
  StudentEnrollmentSchema,
  StudentEnrollmentRangeSchema,
  StudentEnrollmentUpdateSchema,
} from "@/validations/Enrollment/StudentEnrollmentSchema";
import { validate1 } from "@/validations/validator";

class StudentEnrollmentService {
  async getPagedList(params) {
    const result = await StudentEnrollment_repositories.getPagedList(params);
    if (result.success) {
      const pagingObj = result.data || {};
      const rawList = pagingObj.Data ?? pagingObj.data ?? [];
      const totalCount = pagingObj.Count ?? pagingObj.count ?? 0;

      const dto = StudentEnrollmentMapper.mapListApiToClass(rawList);

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
    const payload = StudentEnrollmentMapper.mapCreateToApi(data);
    return await StudentEnrollment_repositories.add(payload);
  }

  async enrollByRange(data) {
    const validation = validate1(data, StudentEnrollmentRangeSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    return await StudentEnrollment_repositories.enrollByRange(
      StudentEnrollmentMapper.mapRangeToApi(data)
    );
  }

  async update(data) {
    const validation = validate1(data, StudentEnrollmentUpdateSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentEnrollmentMapper.mapUpdateToApi(data);
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
