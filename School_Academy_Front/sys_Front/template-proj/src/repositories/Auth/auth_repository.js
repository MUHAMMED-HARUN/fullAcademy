import clsUtil, { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/Auth";

class AuthRepository {
  async login(credentials) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/login`,
      enApiType.post,
      credentials
    );
  }

  async register(userData) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/register`,
      enApiType.post,
      userData
    );
  }

  async getPermissions() {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetPermissions`,
      enApiType.get,
      null
    );
  }

  async getRoles() {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetAllRoles`,
      enApiType.get,
      null
    );
  }

  async getAllRolePermissions(roleId) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/GetAllRolePermissions`,
      enApiType.get,
      { params: { RoleId: roleId } }
    );
  }

  async UpdateRolePermissions(payload) {
    return await clsUtil.FeachRequset(
      `${baseUrl}/UpdateRolePermissions`,
      enApiType.post,
      payload
    );
  }
}

export default new AuthRepository();
