import authRepository from "@/repositories/Auth/auth_repository";
import { loginSchema } from "@/validations/Auth/login";
import { registerSchema } from "@/validations/Auth/registerSchema";
import { getAllRolePermissionsSchema } from "@/validations/Auth/getAllRolePermissionsSchema";
import { updateRolePermissionsSchema } from "@/validations/Auth/UpdateRolePermissionsSchema";
import TResult from "@/Generic/TResult";
import { validate } from "@/validations/validator";
class AuthService {
  async login(credentials) {
    // 1. التحقق من المدخلات عبر Zod
    const validation = validate(loginSchema, credentials);
    if (!validation.success) {
      const errors = validation.errors;
      return new TResult(null, false, errors);
    }
    // 2. استدعاء الـ Repository في حال صحة البيانات
    return await authRepository.login(credentials);
  }

  async register(userData) {
    // 1. التحقق من المدخلات
    const validation = validate(registerSchema, userData);
    registerSchema.safeParse(userData);
    if (!validation.success) {
      const errors = validation.error;
      return new TResult(null, false, errors);
    }

    // 2. تجهيز הـ Payload وتوليد الطلب
    const payload = {
      userName: userData.userName,
      email: userData.email,
      password: userData.password,
      roleID: userData.roleID,
      userType: userData.userType,
      entityID: userData.entityID,
    };

    return await authRepository.register(payload);
  }

  async hasPermission() {}
  async getPermissions() {
    return await authRepository.getPermissions();
  }
  async getRoles() {
    return await authRepository.getRoles();
  }
  async getAllRolePermissions(roleId) {
    const validation = validate(getAllRolePermissionsSchema, { roleId });
    if (!validation.success) {
      const errors = validation.errors;
      return new TResult(null, false, errors);
    }
    return await authRepository.getAllRolePermissions(roleId);
  }
  async UpdateRolePermissions(Changes) {
    const validation = validate(updateRolePermissionsSchema, Changes);
    if (!validation.success) {
      const errors = validation.errors;
      return new TResult(null, false, errors);
    }

    return await authRepository.UpdateRolePermissions(Changes);
  }
}

export default new AuthService();
