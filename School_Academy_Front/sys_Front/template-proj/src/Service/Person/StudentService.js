import TResult from "@/Generic/TResult";
import StudentMapper from "@/Mapper/Person/StudentMapper";
import student_repositories from "@/repositories/Person/StudentRepositories";
import { StudentSchema } from "@/validations/Person/StudentSchema";
import { validate1 } from "@/validations/validator";

class StudentService {
  async getAll(filter) {
    const rees = await student_repositories.getList(filter);
    if (rees.success) {
      const pagingObj = rees.data || {};

      // استخراج القائمة والعدد الإجمالي من كائن Paging<T>
      const rawList = pagingObj.Data;
      const totalCount = pagingObj.Count ?? 0;

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
    var res = await student_repositories.getBy(command);
    var dto;
    if (res.success) {
      dto = StudentMapper.MapFromApiDTO(res.data);
      return new TResult(dto, res.success, res.errors);
    }
    return new TResult(null, false, res.errors);
  }

  async create(data) {
    const validation = validate1(data, StudentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentMapper.mapUpsertToApi(data);
    return await student_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, StudentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = StudentMapper.mapUpsertToApi(data);
    return await student_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await student_repositories.Delete({ StudentID: data });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new StudentService();
