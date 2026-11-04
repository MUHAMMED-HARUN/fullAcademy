import TResult from "@/Generic/TResult";
import SessionMapper from "@/Mapper/Progress/SessionMapper";
import Session_repositories from "@/repositories/Progress/SessionRepositories";
import { SessionSchema } from "@/validations/Progress/SessionSchema";
import { validate1 } from "@/validations/validator";

class SessionService {
  async getAll(params = {}) {
    const rees = await Session_repositories.getListFilter(params);
    if (rees.success) {
      const paging = rees.data || {};
      const items = paging.Data ?? paging.data ?? [];
      return new TResult(
        {
          items: SessionMapper.mapListApiToSessions(items),
          count: paging.Count ?? paging.count ?? 0,
        },
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }

  async searchParents(searchTerm) {
    const term = String(searchTerm ?? "")
      .trim()
      .replace(/[,*|^$!=()/]/g, " ")
      .trim();
    if (!term) return new TResult([], true, null);

    const result = await this.getAll({
      Page: 1,
      PageSize: 10,
      Filter: `SessionName=*${term}`,
      ParentOnly: true,
    });

    if (!result.success) return result;
    return new TResult(result.data?.items || [], true, result.errors);
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
        SessionID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new SessionService();
