// ─────────────────────────────────────────────────────────────────────────────
// AuthMapper — تحويل بيانات Auth بين API و Vue
// نمط: FeeStructureMapper (src/Mapper/Finance/FeeStructureMapper.js)
// ─────────────────────────────────────────────────────────────────────────────
export default class AuthMapper {
  static mapLoginToApi(credentials) {
    return {
      Email: credentials.email?.trim?.() ?? credentials.email,
      Password: credentials.password,
    };
  }

  static mapLoginResult(data) {
    if (!data) return null;
    return {
      token: data.Token ?? null,
      userId: data.UserId ?? null,
      userName: data.UserName ?? null,
      email: data.Email ?? null,
    };
  }

  /**
   * تحويل مصفوفة الأدوار من API
   * @param {Array} dataList
   * @returns {Array}
   */
  static mapRoles(dataList) {
    if (!Array.isArray(dataList)) return [];
    return dataList.map((item) => this.mapRole(item));
  }

  /**
   * تحويل دور واحد من API
   * @param {Object} data
   * @returns {Object}
   */
  static mapRole(data) {
    if (!data) return null;
    return {
      id: data.ID ?? null,
      name: data.Name ?? "",
    };
  }

  /**
   * تحويل مصفوفة الصلاحيات من API
   * @param {Array} dataList
   * @returns {Array}
   */
  static mapPermissions(dataList) {
    if (!Array.isArray(dataList)) return [];
    return dataList.map((item) => this.mapPermission(item));
  }

  /**
   * تحويل صلاحية واحدة من API
   * @param {Object} data
   * @returns {Object}
   */
  static mapPermission(data) {
    if (!data) return null;
    return {
      id: data.Id ?? null,
      name: data.Name ?? "",
      type: data.Type ?? "",
    };
  }

  static mapRolePermissions(dataList) {
    if (!Array.isArray(dataList)) return [];
    return dataList.map((item) => ({
      roleId: item.RoleId ?? "",
      roleName: item.RoleName ?? "",
      permissionName: item.PermissionName ?? "",
    }));
  }

  /**
   * تحويل بيانات التسجيل (Vue → API payload)
   * @param {Object} formData
   * @returns {Object}
   */
  static mapRegisterToApi(formData) {
    return {
      UserName: formData.userName?.trim?.() ?? formData.userName,
      Email: formData.email?.trim?.() ?? formData.email,
      Password: formData.password,
      RoleID: formData.roleID,
      UserType: formData.userType,
      EntityID: formData.entityID,
    };
  }

  static mapRolePermissionChangesToApi(changes) {
    return {
      Changes: changes.map((change) => ({
        RoleId: change.roleId,
        PermissionName: change.permissionName,
        IsDelete: change.isDelete,
      })),
    };
  }
}
