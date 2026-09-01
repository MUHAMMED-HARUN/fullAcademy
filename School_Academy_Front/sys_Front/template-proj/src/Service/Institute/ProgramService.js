import TResult from "@/Generic/TResult";
import ProgramMapper from "@/Mapper/Institute/ProgramMapper";
import program_repositories from "@/repositories/Institute/ProgramRepositories";
import { ProgramuteSchema } from "@/validations/Institute/ProgramSchema";
import { validate1 } from "@/validations/validator";
class ProgramService {
  async getAll() {
    const rees = await program_repositories.getList();
    var dto;
    if (rees.success) {
      dto = ProgramMapper.mapListApiToProgram(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }
  async getBy(command) {
    return await program_repositories.getBy(command);
  }

  async create(data) {
    const validation = validate1(data, ProgramuteSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = ProgramMapper.mapUpsertToApi(data);
    return await program_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, ProgramuteSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = ProgramMapper.mapUpsertToApi(data);
    return await program_repositories.update(payload);
  }
  async delete(data) {
    if (data) {
      return await program_repositories.Delete({ ProgramID: data });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new ProgramService();
