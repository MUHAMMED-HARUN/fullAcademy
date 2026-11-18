import TResult from "@/Generic/TResult";
import AssignmentNominationMapper from "@/Mapper/Assigment/AssignmentNominationMapper";
import AssignmentNominationRepositories from "@/repositories/Assigment/AssignmentNominationRepositories";
import { AssignmentNominationSchema } from "@/validations/Assignment/AssignmentNominationSchema";
import { validate1 } from "@/validations/validator";

class AssignmentNominationinfoService {
  async getAll(params = {}) {
    const response = await AssignmentNominationRepositories.getList(params);
    if (!response.success) return new TResult(null, false, response.errors);
    const paging = response.data || {};
    return new TResult(
      {
        items: AssignmentNominationMapper.mapListApiToAssignmentNomination(
          paging.Data ?? paging.data ?? []
        ),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
  }

  async getBy(command) {
    const rees = await AssignmentNominationRepositories.getBy(command);
    var dto;
    if (rees.success) {
      dto = AssignmentNominationMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, AssignmentNominationSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentNominationMapper.mapUpsertToApi(data);
    return await AssignmentNominationRepositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, AssignmentNominationSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentNominationMapper.mapUpsertToApi(data);
    return await AssignmentNominationRepositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await AssignmentNominationRepositories.Delete({
        AssignmentNominationID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new AssignmentNominationinfoService();
