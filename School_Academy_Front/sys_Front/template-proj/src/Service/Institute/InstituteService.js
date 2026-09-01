import TResult from "@/Generic/TResult";
import InstituteMapper from "@/Mapper/Institute/InstituteMapper";
import institute_repositories from "@/repositories/Institute/InstituteRepositories";
import { InstituteSchema } from "@/validations/Institute/InstituteSchema";
import { validate1 } from "@/validations/validator";
class InstituteService {
  async getAll() {
    const rees = await institute_repositories.getList();
    var dto;
    if (rees.success) {
      dto = InstituteMapper.mapListApiToInstitute(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getBy(command) {
    return await institute_repositories.getBy(command);
  }

  async create(data) {
    const validation = validate1(data, InstituteSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = InstituteMapper.mapUpsertToApi(data);
    return await institute_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, InstituteSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = InstituteMapper.mapUpsertToApi(data);
    return await institute_repositories.update(payload);
  }
  async delete(data) {
    if (data) {
      return await institute_repositories.Delete({ InstituteID: data });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new InstituteService();
