import TResult from "@/Generic/TResult";
import TeachingAssignmentMapper from "@/Mapper/Enrollment/TeachingAssignmentMapper";
import TeachingAssignment_repositories from "@/repositories/Enrollment/TeachingAssignmentRepositories";
import { TeachingAssignmentSchema } from "@/validations/Enrollment/TeachingAssignmentSchema";
import { validate1 } from "@/validations/validator";

class TeachingAssignmentService {
  async getAll(queryParams, role) {
    var result = new TResult(null, false, "لا يوجد دور");
    if (role == "Teacher") {
      result = await TeachingAssignment_repositories.GetTeacherGroups(
        queryParams
      );
    } else if (role == "Admin") {
      result = await TeachingAssignment_repositories.getList(queryParams);
    }
    if (result.success) {
      const pagingObj = result.data || {};

      // استخراج القائمة والعدد الإجمالي من كائن Paging<T>
      const rawList = pagingObj.Data;
      const totalCount = pagingObj.Count ?? 0;

      // تحويل مصفوفة الطلاب عبر الـ Mapper
      const dto = TeachingAssignmentMapper.mapListApiToTeachers(rawList);

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
    return await TeachingAssignment_repositories.getBy(command);
  }

  async create(data) {
    const validation = validate1(data, TeachingAssignmentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = TeachingAssignmentMapper.mapUpsertToApi(data);
    return await TeachingAssignment_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, TeachingAssignmentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = TeachingAssignmentMapper.mapUpsertToApi(data);
    return await TeachingAssignment_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await TeachingAssignment_repositories.Delete({
        EnrollmentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new TeachingAssignmentService();
