import TResult from "@/Generic/TResult";
import StudentInvoiceMapper from "@/Mapper/Finance/StudentInvoiceMapper";
import StudentInvoiceRepositories from "@/repositories/Finance/StudentInvoiceRepositories";
import {
  NewStudentInvoiceSchema,
  UpdateStudentInvoiceSchema,
} from "@/validations/Finance/StudentInvoiceSchema";
import { validate1 } from "@/validations/validator";

class StudentInvoiceService {
  async getAll() {
    const result = await StudentInvoiceRepositories.getList();
    if (result.success) {
      const dto = StudentInvoiceMapper.mapListApiToClass(result.data);
      return new TResult(dto, result.success, result.errors);
    }
    return new TResult(null, false, result.errors);
  }

  async getBy(command) {
    const result = await StudentInvoiceRepositories.getBy(command);
    if (result.success) {
      return new TResult(
        StudentInvoiceMapper.MapFromApiDTO(result.data),
        result.success,
        result.errors
      );
    }
    return new TResult(null, false, result.errors);
  }

  async create(data) {
    const validation = validate1(data, NewStudentInvoiceSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = StudentInvoiceMapper.mapNewToApi(data);
    return await StudentInvoiceRepositories.add(payload);
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
