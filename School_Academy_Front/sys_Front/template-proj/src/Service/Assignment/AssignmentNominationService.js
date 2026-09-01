import TResult from "@/Generic/TResult";
import AssignmentNominationMapper from "@/Mapper/Assigment/AssignmentNominationMapper";
import AssignmentNominationRepositories from "@/repositories/Assigment/AssignmentNominationRepositories";
import { AssignmentNominationSchema } from "@/validations/Assignment/AssignmentNominationSchema";
import { validate1 } from "@/validations/validator";

class AssignmentNominationinfoService {
  async getAll() {
    const rees = await AssignmentNominationRepositories.getList();
    var dto;
    if (rees.success) {
      dto = AssignmentNominationMapper.mapListApiToAssignmentNomination(
        rees.data
      );
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
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
      console.log("valid nomin", validation);
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentNominationMapper.mapUpsertToApi(data);
    console.log("payload nomin", payload);

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
