import TResult from "@/Generic/TResult";
import TeacherMapper from "@/Mapper/Person/TeacherMapper";
import Teacher_repositories from "@/repositories/Person/TeacherRepositories";
import {
  NewTeacherSchema,
  TeacherLookupSchema,
  UpdateTeacherSchema,
} from "@/validations/Person/TeacherSchema";
import { GuidSchema } from "@/validations/Person/guidSchema";
import { validate1 } from "@/validations/validator";

class TeacherService {
  async getAll(filter) {
    const rees = await Teacher_repositories.getList(filter);
    if (rees.success) {
      return new TResult(
        TeacherMapper.mapPagingApiToTeacher(rees.data),
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const validation = validate1(command, TeacherLookupSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const result = await Teacher_repositories.getBy(command);
    if (result.success) {
      const dto = TeacherMapper.MapFromApiDTO(result.data);
      return new TResult(dto, result.success, result.errors);
    }
    return new TResult(null, false, result.errors);
  }

  async create(data) {
    const validation = validate1(data, NewTeacherSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const payload = TeacherMapper.mapUpsertToApi(data);
    return await Teacher_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, UpdateTeacherSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const payload = TeacherMapper.mapUpsertToApi(data);
    return await Teacher_repositories.update(payload);
  }

  async delete(data) {
    const validation = GuidSchema.safeParse(data);
    if (validation.success) {
      return await Teacher_repositories.Delete({ TeacherID: data });
    }
    return new TResult(null, false, {
      teacherID: validation.error.issues[0]?.message || "معرّف المعلم غير صالح",
    });
  }
}

export default new TeacherService();
