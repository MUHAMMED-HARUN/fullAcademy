import authRepository from "@/repositories/Auth/auth_repository";
import { loginSchema } from "@/validations/Auth/login";
import { registerSchema } from "@/validations/Auth/registerSchema";
import { getAllRolePermissionsSchema } from "@/validations/Auth/getAllRolePermissionsSchema";
import { updateRolePermissionsSchema } from "@/validations/Auth/UpdateRolePermissionsSchema";
import TResult from "@/Generic/TResult";
import { validate1 } from "@/validations/validator";
import AuthMapper from "@/Mapper/Auth/AuthMapper";

class AuthService {
  async login(credentials) {
    const validation = validate1(credentials, loginSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const result = await authRepository.login(
      AuthMapper.mapLoginToApi(credentials)
    );
    if (result.success && result.data) {
      result.data = AuthMapper.mapLoginResult(result.data);
    }
    return result;
  }

  async register(userData) {
    const validation = validate1(userData, registerSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const payload = AuthMapper.mapRegisterToApi(userData);
    return await authRepository.register(payload);
  }

  async getPermissions() {
    const result = await authRepository.getPermissions();
    if (result.success && Array.isArray(result.data)) {
      result.data = AuthMapper.mapPermissions(result.data);
    }
    return result;
  }

  async getRoles() {
    const result = await authRepository.getRoles();
    if (result.success && Array.isArray(result.data)) {
      result.data = AuthMapper.mapRoles(result.data);
    }
    return result;
  }

  async getAllRolePermissions(roleId) {
    const validation = validate1({ roleId }, getAllRolePermissionsSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const result = await authRepository.getAllRolePermissions(roleId);
    if (result.success && Array.isArray(result.data)) {
      result.data = AuthMapper.mapRolePermissions(result.data);
    }
    return result;
  }

  async UpdateRolePermissions(Changes) {
    const validation = validate1(Changes, updateRolePermissionsSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    return await authRepository.UpdateRolePermissions(
      AuthMapper.mapRolePermissionChangesToApi(Changes.changes)
    );
  }
}

export default new AuthService();
