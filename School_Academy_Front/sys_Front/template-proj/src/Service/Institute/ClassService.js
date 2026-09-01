import TResult from "@/Generic/TResult";
import ClassMapper from "@/Mapper/Institute/ClassMapper";
import Class_repositories from "@/repositories/Institute/ClassRepositories";
import { ClassSchema } from "@/validations/Institute/ClassSchema";
import { validate1 } from "@/validations/validator";
class ClassService {
  async getAll() {
    const rees = await Class_repositories.getList();
    var dto;
    if (rees.success) {
      dto = ClassMapper.mapListApiToClass(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getBy(command) {
    return await Class_repositories.getBy(command);
  }

  async create(data) {
    const validation = validate1(data, ClassSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = ClassMapper.mapUpsertToApi(data);
    return await Class_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, ClassSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = ClassMapper.mapUpsertToApi(data);
    return await Class_repositories.update(payload);
  }
  async delete(data) {
    if (data) {
      return await Class_repositories.Delete({ ClassID: data });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new ClassService();
