import TResult from "@/Generic/TResult";
import AssignmentMapper from "@/Mapper/Assigment/AssigmentMapper";
import Assignment_repositories from "@/repositories/Assigment/AssignmentRepositories";
import {
  AssignmentSchema,
  AssignmentUpdateSchema,
} from "@/validations/Assignment/AssignmentSchema";
import { validate1 } from "@/validations/validator";

class AssignmentService {
  async getAll(params = {}) {
    const response = await Assignment_repositories.getList(params);
    if (!response.success) return new TResult(null, false, response.errors);
    const paging = response.data || {};
    return new TResult(
      {
        items: AssignmentMapper.mapListApiToAssignments(
          paging.Data ?? paging.data ?? []
        ),
        count: paging.Count ?? paging.count ?? 0,
      },
      true,
      response.errors
    );
  }

  async getBy(command) {
    const rees = await Assignment_repositories.getBy(command);
    var dto;
    if (rees.success) {
      dto = AssignmentMapper.MapFromApiDTO(rees.data);
      return new TResult(dto, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getForUpsert(assignmentID) {
    const response = await Assignment_repositories.getForUpsert(assignmentID);
    return response.success
      ? new TResult(
          AssignmentMapper.MapFromApiDTO(response.data),
          true,
          response.errors
        )
      : new TResult(null, false, response.errors);
  }

  async create(data) {
    const validation = validate1(data, AssignmentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentMapper.mapCreateToApi(data);
    return await Assignment_repositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, AssignmentUpdateSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = AssignmentMapper.mapUpdateToApi(data);
    return await Assignment_repositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await Assignment_repositories.Delete({
        AssignmentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new AssignmentService();
