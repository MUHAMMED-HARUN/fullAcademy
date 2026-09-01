import TResult from "@/Generic/TResult";
import AssignmentScopeinfoMapper from "@/Mapper/Assigment/AssignmentScopeinfoMapper";
import AssignmentScopeinfo_repositories from "@/repositories/Assigment/AssignmentScopeinfoRepositories";
import { AssignmentScopeInfoSchema } from "@/validations/Assignment/AssignmentScopeinfoSchema";
import { validate1 } from "@/validations/validator";

class AssignmentScopeinfoService {
  async getAll() {
    const rees = await AssignmentScopeinfo_repositories.getList();
    var dto;
    if (rees.success) {
      dto = AssignmentScopeinfoMapper.mapListApiToAssignmentScopeInfos(
        rees.data
      );
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const rees = await AssignmentScopeinfo_repositories.getBy(command);
    var dto;
    console.log(rees);
    if (rees.success) {
      dto = AssignmentScopeinfoMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    alert("s");
    const validation = validate1(data, AssignmentScopeInfoSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentScopeinfoMapper.mapUpsertToApi(data);
    return await AssignmentScopeinfo_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, AssignmentScopeInfoSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentScopeinfoMapper.mapUpsertToApi(data);
    return await AssignmentScopeinfo_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await AssignmentScopeinfo_repositories.Delete({
        AssignmentScopeInfoID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new AssignmentScopeinfoService();
