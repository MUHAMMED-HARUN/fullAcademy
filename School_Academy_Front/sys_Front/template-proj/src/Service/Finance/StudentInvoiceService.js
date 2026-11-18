import TResult from "@/Generic/TResult";
import StudentInvoiceMapper from "@/Mapper/Finance/StudentInvoiceMapper";
import StudentInvoiceRepositories from "@/repositories/Finance/StudentInvoiceRepositories";
import {
  GenerateStudentInvoiceSchema,
  UpdateStudentInvoiceSchema,
} from "@/validations/Finance/StudentInvoiceSchema";
import { validate1 } from "@/validations/validator";

class StudentInvoiceService {
  async getAllFilter(params = {}) {
    const result = await StudentInvoiceRepositories.getListFilter(params);
    if (!result.success) return new TResult(null, false, result.errors);
    return new TResult(
      StudentInvoiceMapper.mapPagingApiToClass(result.data),
      true,
      result.errors
    );
  }

  async getPayableInvoices(params = {}) {
    return await this.getAllFilter({ ...params, PayableOnly: true });
  }

  async getPayableInvoices(params = {}) {
    return await this.getAllFilter({ ...params, PayableOnly: true });
  }

  async getBy(command) {
    const result = await StudentInvoiceRepositories.getBy(command);
    if (result.success) {
      return new TResult(
        StudentInvoiceMapper.mapListApiToClass(result.data),
        result.success,
        result.errors
      );
    }
    return new TResult(null, false, result.errors);
  }

  async getForUpsert(id) {
    const result = await StudentInvoiceRepositories.getForUpsert(id);
    if (!result.success) return new TResult(null, false, result.errors);
    const raw = result.data?.[0] ?? result.data;
    return new TResult(
      StudentInvoiceMapper.MapFromApiDTO(raw),
      true,
      result.errors
    );
  }

  async getByAcademicEnrollment(id) {
    const result = await StudentInvoiceRepositories.getByAcademicEnrollment(id);
    if (!result.success) return new TResult(null, false, result.errors);
    return new TResult(
      StudentInvoiceMapper.mapListApiToClass(result.data),
      true,
      result.errors
    );
  }

  async getFinanceSummary(studentID) {
    const result = await StudentInvoiceRepositories.getFinanceSummary(
      studentID
    );
    return result.success
      ? new TResult(
          StudentInvoiceMapper.mapFinanceSummary(result.data),
          true,
          result.errors
        )
      : new TResult(null, false, result.errors);
  }

  async create(data) {
    const validation = validate1(data, GenerateStudentInvoiceSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentInvoiceMapper.mapNewToApi(data);
    return await StudentInvoiceRepositories.generateForEnrollment(payload);
  }

  async update(data) {
    const validation = validate1(data, UpdateStudentInvoiceSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentInvoiceMapper.mapUpdateToApi(data);
    return await StudentInvoiceRepositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await StudentInvoiceRepositories.Delete({
        StudentInvoiceID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new StudentInvoiceService();
