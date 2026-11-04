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
      const rawList = pagingObj.Data;
      const totalCount = pagingObj.Count ?? 0;
      const dto = GroupMapper.mapListApiToClass(rawList);
      return new TResult(
        { items: dto, count: totalCount },
        rees.success,
        rees.errors
      );
    }
    return new TResult(null, false, rees.errors);
  }

  async GetGroupsByTeacher(teacherID) {
    const rees = await Group_repositories.GetGroupsByTeacherID({
      TeacherID: teacherID,
    });
    var dto;
    if (rees.success) {
      dto = GroupMapper.MapFromApiclsGroupList(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
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
