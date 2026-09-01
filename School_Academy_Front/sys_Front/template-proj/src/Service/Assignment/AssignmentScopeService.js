import TResult from "@/Generic/TResult";
import AssignmentScopeMapper from "@/Mapper/Assigment/AssignmentScopeMapper";
import AssignmentScope_repositories from "@/repositories/Assigment/AssignmentScopeRepositories copy";
import { AssignmentScopeSchema } from "@/validations/Assignment/AssignmentScopeSchema";
import { validate1 } from "@/validations/validator";

class AssignmentScopeService {
  async getAll() {
    const rees = await AssignmentScope_repositories.getList();
    var dto;
    if (rees.success) {
      dto = AssignmentScopeMapper.mapListApiToAssignmentScopes(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const rees = await AssignmentScope_repositories.getBy(command);
    var dto;
    console.log(rees);
    if (rees.success) {
      dto = AssignmentScopeMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async GetCanNominateList(command) {
    const rees = await AssignmentScope_repositories.GetCanNominateList(command);
    var dto;
    if (rees.success) {
      dto = AssignmentScopeMapper.mapListApiToCanNomenate(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, AssignmentScopeSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentScopeMapper.mapUpsertToApi(data);
    return await AssignmentScope_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, AssignmentScopeSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentScopeMapper.mapUpsertToApi(data);
    return await AssignmentScope_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await AssignmentScope_repositories.Delete({
        AssignmentScopeID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new AssignmentScopeService();
