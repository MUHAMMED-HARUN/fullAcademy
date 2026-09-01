import TResult from "@/Generic/TResult";
import InstituteClassMapper from "@/Mapper/Institute/InstituteClassMapper";
import InstituteClass_repositories from "@/repositories/Institute/InstituteClassRepository";
import { InstituteClassSchema } from "@/validations/Institute/InstituteClassSchema";
import { validate1 } from "@/validations/validator";
class InstituteClassService {
  async getAll() {
    const rees = await InstituteClass_repositories.getList();
    var dto;
    if (rees.success) {
      dto = InstituteClassMapper.mapListApiToInstituteClass(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getBy(command) {
    var res = await InstituteClass_repositories.getBy(command);
    if (res.success) {
      var dto = InstituteClassMapper.MapFromApiDTO(res.data);
      return new TResult(dto, res.success, null);
    }
    return new TResult(null, false, res.errors);
  }

  async create(data) {
    const validation = validate1(data, InstituteClassSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = InstituteClassMapper.mapUpsertToApi(data);
    return await InstituteClass_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, InstituteClassSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = InstituteClassMapper.mapUpsertToApi(data);
    return await InstituteClass_repositories.update(payload);
  }
  async delete(data) {
    if (data) {
      return await InstituteClass_repositories.Delete({
        InstituteClassID: data,
      });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new InstituteClassService();
