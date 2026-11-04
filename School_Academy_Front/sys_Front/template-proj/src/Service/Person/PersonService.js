import TResult from "@/Generic/TResult";
import PersonMapper from "@/Mapper/Person/PersonMapper";
import Person_repositories from "@/repositories/Person/PersonRepositories";
import {
  PersonForUpsertIdSchema,
  PersonSearchSchema,
  NewPersonSchema,
  UpdatePersonSchema,
} from "@/validations/Person/PersonSchema";
import { validate1 } from "@/validations/validator";

class PersonService {
  async getAll(filter) {
    const rees = await Person_repositories.getList(filter);
    if (rees.success) {
      const pagingObj = rees.data || {};
      // استخراج القائمة والعدد الإجمالي من كائن Paging<T>
      const rawList = pagingObj.Data || pagingObj.data || [];
      const totalCount = pagingObj.Count ?? 0;

      // تحويل البيانات عبر الـ Mapper
      const dto = PersonMapper.mapListApiToPerson(rawList);

      return new TResult(
        { items: dto, count: totalCount },
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }

  async getBy(command) {
    return await Person_repositories.getBy(command);
  }

  async getForUpsert(personID) {
    const validation = PersonForUpsertIdSchema.safeParse(personID);
    if (!validation.success) {
      return new TResult(null, false, {
        personID: validation.error.issues[0]?.message || "معرّف الشخص غير صالح",
      });
    }

    const result = await Person_repositories.getForUpsert(personID);
    if (!result.success) return result;

    return new TResult(
      PersonMapper.MapForUpsertFromApiDTO(result.data),
      result.success,
      result.errors
    );
  }

  async searchForCandidates(criteria) {
    const validation = validate1(criteria, PersonSearchSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const value = criteria.value.trim();
    return await this.getAll({
      Page: 1,
      PageSize: 10,
      Filter: `${criteria.filterType}=*${value}`,
    });
  }

  async create(data) {
    const validation = validate1(data, NewPersonSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = PersonMapper.mapUpsertToApi(data);
    return await Person_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, UpdatePersonSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = PersonMapper.mapUpsertToApi(data);
    return await Person_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await Person_repositories.Delete({ PersonID: data });
    }
    return new TResult(null, false, ["Invalid data for deletion"]);
  }
}

export default new PersonService();
