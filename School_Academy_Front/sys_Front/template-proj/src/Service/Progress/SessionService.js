import TResult from "@/Generic/TResult";
import SessionMapper from "@/Mapper/Progress/SessionMapper";
import Session_repositories from "@/repositories/Progress/SessionRepositories";
import { SessionSchema } from "@/validations/Progress/SessionSchema";
import { validate1 } from "@/validations/validator";

class SessionService {
  async getAll() {
    const rees = await Session_repositories.getList();
    var dto;
    if (rees.success) {
      dto = SessionMapper.mapListApiToSessions(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    const rees = await Session_repositories.getBy(command);
    var dto;
    if (rees.success) {
      dto = SessionMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, SessionSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = SessionMapper.mapUpsertToApi(data);
    return await Session_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, SessionSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = SessionMapper.mapUpsertToApi(data);
    return await Session_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await Session_repositories.Delete({
        EnrollmentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new SessionService();
