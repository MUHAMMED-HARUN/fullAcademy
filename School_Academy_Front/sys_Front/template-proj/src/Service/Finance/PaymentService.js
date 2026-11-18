import TResult from "@/Generic/TResult";
import PaymentMapper from "@/Mapper/Finance/PaymentMapper";
import PaymentRepositories from "@/repositories/Finance/PaymentRepositories";
import {
  PaymentSchema,
  ReversePaymentSchema,
} from "@/validations/Finance/PaymentSchema";
import { validate1 } from "@/validations/validator";

class PaymentService {
  async getAllFilter(params = {}) {
    const result = await PaymentRepositories.getListFilter(params);
    if (!result.success) return new TResult(null, false, result.errors);
    return new TResult(PaymentMapper.mapPagingApiToClass(result.data), true, result.errors);
  }

  async getBy(command) {
    const result = await PaymentRepositories.getBy(command);
    return result.success
      ? new TResult(PaymentMapper.MapFromApiDTO(result.data), true, result.errors)
      : new TResult(null, false, result.errors);
  }

  async getPaymentsByInvoice(invoiceID) {
    const result = await PaymentRepositories.getPaymentsByInvoice(invoiceID);
    return result.success
      ? new TResult(PaymentMapper.mapListApiToClass(result.data), true, result.errors)
      : new TResult(null, false, result.errors);
  }

  async create(data) {
    const validation = validate1(data, PaymentSchema);
    if (!validation.isValid) return new TResult(null, false, validation.errors);
    return await PaymentRepositories.add(PaymentMapper.mapNewToApi(data));
  }

  async reverse(data) {
    const validation = validate1(data, ReversePaymentSchema);
    if (!validation.isValid) return new TResult(null, false, validation.errors);
    return await PaymentRepositories.reverse(PaymentMapper.mapReverseToApi(data));
  }
}

export default new PaymentService();
