import TResult from "@/Generic/TResult";
import PaymentMapper from "@/Mapper/Finance/PaymentMapper";
import PaymentRepositories from "@/repositories/Finance/PaymentRepositories";
import { PaymentSchema } from "@/validations/Finance/PaymentSchema";
import { validate1 } from "@/validations/validator";

class PaymentService {
  async getAll() {
    const result = await PaymentRepositories.getList();
    if (result.success) {
      const dto = PaymentMapper.mapListApiToClass(result.data);
      return new TResult(dto, result.success, result.errors);
    }
    return new TResult(null, false, result.errors);
  }

  async getBy(command) {
    const result = await PaymentRepositories.getBy(command);
    if (result.success) {
      return new TResult(
        PaymentMapper.MapFromApiDTO(result.data),
        result.success,
        result.errors
      );
    }
    return new TResult(null, false, result.errors);
  }

  async create(data) {
    const validation = validate1(data, PaymentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = PaymentMapper.mapUpsertToApi(data);
    return await PaymentRepositories.add(payload);
  }

  async update(data) {
    const validation = validate1(data, PaymentSchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = PaymentMapper.mapUpsertToApi(data);
    return await PaymentRepositories.update(payload);
  }

  async delete(data) {
    if (data) {
      return await PaymentRepositories.Delete({
        PaymentID: data,
      });
    }
    return new TResult(null, false, ["بيانات غير صالحة للحذف"]);
  }
}

export default new PaymentService();
