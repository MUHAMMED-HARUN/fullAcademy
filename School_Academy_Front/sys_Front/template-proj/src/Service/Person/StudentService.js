import TResult from "@/Generic/TResult";
import StudentMapper from "@/Mapper/Person/StudentMapper";
import student_repositories from "@/repositories/Person/StudentRepositories";
import {
  NewStudentSchema,
  StudentLookupSchema,
  UpdateStudentSchema,
} from "@/validations/Person/StudentSchema";
import { GuidSchema } from "@/validations/Person/guidSchema";
import { validate1 } from "@/validations/validator";

class StudentService {
  async getAll(filter) {
    const rees = await student_repositories.getList(filter);
    if (rees.success) {
      const pagingObj = rees.data || {};

      // استخراج القائمة والعدد الإجمالي من كائن Paging<T>
      const rawList = pagingObj.Data || pagingObj.data || [];
      const totalCount = pagingObj.Count ?? pagingObj.count ?? 0;

      // تحويل مصفوفة الطلاب عبر الـ Mapper
      const dto = StudentMapper.mapListApiToStudent(rawList);

      // تمرير المابنج مع إجمالي العدد
      return new TResult(
        { items: dto, count: totalCount },
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const validation = validate1(command, StudentLookupSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const result = await student_repositories.getBy(command);
    if (!result.success) return result;

    return new TResult(
      StudentMapper.MapFromApiDTO(result.data),
      result.success,
      result.errors
    );
  }

  async create(data) {
    const validation = validate1(data, NewStudentSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const payload = StudentMapper.mapUpsertToApi(data);
    return await student_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, UpdateStudentSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const payload = StudentMapper.mapUpsertToApi(data);
    return await student_repositories.update(payload);
  }

  async delete(data) {
    const validation = GuidSchema.safeParse(data);
    if (validation.success) {
      return await student_repositories.Delete({ StudentID: data });
    }
    return new TResult(null, false, {
      studentID: validation.error.issues[0]?.message || "معرّف الطالب غير صالح",
    });
  }
}

export default new StudentService();
