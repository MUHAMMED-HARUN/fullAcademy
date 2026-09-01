import TResult from "@/Generic/TResult";
import AssignmentMapper from "@/Mapper/Assigment/AssigmentMapper";
import Assignment_repositories from "@/repositories/Assigment/AssignmentRepositories";
import { AssignmentSchema } from "@/validations/Assignment/AssignmentSchema";
import { validate1 } from "@/validations/validator";

class AssignmentService {
  async getAll() {
    const rees = await Assignment_repositories.getList();
    var dto;
    if (rees.success) {
      dto = AssignmentMapper.mapListApiToAssignments(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const rees = await Assignment_repositories.getBy(command);
    var dto;
    console.log(rees);
    if (rees.success) {
      dto = AssignmentMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, AssignmentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentMapper.mapUpsertToApi(data);
    return await Assignment_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, AssignmentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentMapper.mapUpsertToApi(data);
    return await Assignment_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await Assignment_repositories.Delete({
        AssignmentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new AssignmentService();
