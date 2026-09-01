import TResult from "@/Generic/TResult";
import api from "@/api/axios";
class AuthRepository {
  async login(credentials) {
    try {
      const response = await api.post(`/Auth/login`, credentials);
      const { Value, IsSuccess, Error } = response.data;

      return new TResult(Value, IsSuccess, Error ? { general: Error } : {});
    } catch (err) {
      const serverError =
        err.response?.data?.Error || "حدث خطأ في الاتصال بالخادم";
      return new TResult(null, false, { general: serverError });
    }
  }

  async register(userData) {
    try {
      console.log(userData);
      const response = await api.post(`/Auth/register`, userData);
      const { Value, IsSuccess, Error } = response.data;

      return new TResult(Value, IsSuccess, Error ? { general: Error } : {});
    } catch (err) {
      const serverError =
        err.response?.data?.Error || "حدث خطأ أثناء إنشاء الحساب";
      return new TResult(null, false, { general: serverError });
    }
  }
  async getPermissions() {
    try {
      const response = await api.get(`/Auth/GetPermissions`);
      const { Value, IsSuccess, Error } = response.data;
      return new TResult(Value, IsSuccess, Error ? { general: Error } : {});
    } catch (err) {
      const serverError = err.response?.data?.Error || "خطا  في الخادم";
      return new TResult(null, false, { general: serverError });
    }
  }
  async getRoles() {
    try {
      const response = await api.get(`/Auth/GetAllRoles`);
      const { Value, IsSuccess, Error } = response.data;
      return new TResult(Value, IsSuccess, Error ? { general: Error } : {});
    } catch (err) {
      const serverError = err.response?.data?.Error || "خطا  في الخادم";
      return new TResult(null, false, { general: serverError });
    }
  }
  async getAllRolePermissions(roleId) {
    try {
      const response = await api.get(`/Auth/GetAllRolePermissions`, {
        params: { roleId },
      });
      const { Value, IsSuccess, Error } = response.data;
      return new TResult(Value, IsSuccess, Error ? { general: Error } : {});
    } catch (err) {
      const serverError = err.response?.data?.Error || "خطا  في الخادم";
      return new TResult(null, false, { general: serverError });
    }
  }
  async UpdateRolePermissions(payload) {
    try {
      const response = await api.post(`/Auth/UpdateRolePermissions/`, payload);
      const { Value, IsSuccess, Error } = response.data;
      return new TResult(Value, IsSuccess, Error ? { general: Error } : {});
    } catch (err) {
      const serverError = err.response?.data?.Error || "خطا  في الخادم";
      return new TResult(null, false, { general: serverError });
    }
  }
}

export default new AuthRepository();
