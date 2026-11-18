import TResult from "@/Generic/TResult";
import GroupMapper from "@/Mapper/Institute/GroupMapper";
import Group_repositories from "@/repositories/Institute/GroupRepositories";
import { GroupSchema } from "@/validations/Institute/GroupSchema";
import { validate1 } from "@/validations/validator";

class GroupService {
  async getAll(filter) {
    const rees = await Group_repositories.getList(filter);
    if (rees.success) {
      const pagingObj = rees.data || {};
      const rawList = pagingObj.Data ?? pagingObj.data ?? [];
      const totalCount = pagingObj.Count ?? pagingObj.count ?? 0;
      const dto = GroupMapper.mapListApiToClass(rawList);
      return new TResult(
        { items: dto, count: totalCount },
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }

  async getMyTeachingGroups() {
    const response = await Group_repositories.getMyTeachingGroups({
      Page: 1,
      PageSize: 100,
    });
    if (!response.success) return new TResult(null, false, response.errors);
    const paging = response.data || {};
    return new TResult(
      {
        items: GroupMapper.mapListApiToClass(paging.Data ?? paging.data ?? []),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
  }

  async getBy(command) {
    const rees = await Group_repositories.getBy(command);
    var dto;
    if (rees.success) {
      dto = GroupMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async create(data) {
    const validation = validate1(data, GroupSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = GroupMapper.mapUpsertToApi(data);
    return await Group_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, GroupSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = GroupMapper.mapUpsertToApi(data);
    return await Group_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await Group_repositories.Delete({ GroupID: data });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new GroupService();
