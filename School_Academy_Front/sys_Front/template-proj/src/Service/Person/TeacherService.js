import TResult from "@/Generic/TResult";
import TeacherMapper from "@/Mapper/Person/TeacherMapper";
import Teacher_repositories from "@/repositories/Person/TeacherRepositories";
import { TeacherSchema } from "@/validations/Person/TeacherSchema";
import { validate1 } from "@/validations/validator";

class TeacherService {
  async getAll(filter) {
    const rees = await Teacher_repositories.getList(filter);
    if (rees.success) {
      const pagingObj = rees.data || {};

      // استخراج القائمة والعدد الإجمالي من كائن Paging<T>
      const rawList = pagingObj.Data || pagingObj.data || [];
      const totalCount = pagingObj.Count ?? pagingObj.count ?? 0;

      // تحويل البيانات عبر الـ Mapper
      const dto = TeacherMapper.mapListApiToTeacher(rawList);

      return new TResult(
        { items: dto, count: totalCount },
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    var res = await Teacher_repositories.getBy(command);
    if (res.success) {
      const dto = TeacherMapper.MapFromApiDTO(res.data);
      return new TResult(dto, res.success, res.errors);
    }
    return new TResult(null, false, res.errors);
  }

  async create(data) {
    const validation = validate1(data, TeacherSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = TeacherMapper.mapUpsertToApi(data);
    return await Teacher_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, TeacherSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = TeacherMapper.mapUpsertToApi(data);
    return await Teacher_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await Teacher_repositories.Delete({ TeacherID: data });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new TeacherService();
