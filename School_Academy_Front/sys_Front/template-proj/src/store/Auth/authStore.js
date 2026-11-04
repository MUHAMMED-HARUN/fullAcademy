import { defineStore } from "pinia";
import { jwtDecode } from "jwt-decode";
import authService from "@/Service/Auth/authService";

const decodeToken = (token) => {
  if (!token) return {};
  try {
    return jwtDecode(token);
  } catch {
    return {};
  }
};

const claimValues = (value) => {
  if (Array.isArray(value))
    return value.filter((item) => typeof item === "string");
  return typeof value === "string" && value ? [value] : [];
};

const normalizePermission = (permission) =>
  ({
    "Permissions.Users.View": "Can_Read_User",
    "Permissions.Users.Create": "Can_Register_User",
    "Permissions.Users.Update": "Can_Update_User",
    "Permissions.Users.Delete": "Can_Delete_User",
    "Permissions.Users.SetPermission": "Can_SetPermission_User",
  }[permission] || permission);

const permissionValues = (value) => claimValues(value).map(normalizePermission);

export const useAuthStore = defineStore("auth", {
  state: () => {
    const token = localStorage.getItem("token") || null;
    const claims = decodeToken(token);
    return {
      user: JSON.parse(localStorage.getItem("user")) || null,
      token,
      roles: claimValues(claims.Role),
      permissions: permissionValues(claims.Permission),
      loading: false,
      saving: false,
      error: null,
    };
  },

  getters: {
    isAuthenticated: (state) => !!state.token,
    isAdmin: (state) =>
      state.roles.some((role) => role.toLowerCase() === "admin"),
    userData: (state) => {
      if (!state.token) return null;
      const claims = decodeToken(state.token);
      return {
        id: claims.UserId || claims.nameid || claims.sub || null,
        name: claims.UserName || claims.unique_name || claims.name || null,
        email: claims.Email || claims.email || null,
        role: claimValues(claims.Role)[0] || null,
        entityId: claims.EntityId || null,
        entityType: claims.EntityType || null,
        issuer: claims.iss || null,
        audience: claims.aud || null,
        expiresAt: claims.exp ? new Date(claims.exp * 1000) : null,
      };
    },
    UserRole: (state) => state.roles[0] || null,
    UserEntityID: (state) => decodeToken(state.token).EntityId || null,
  },

  actions: {
    hasPermission(permission) {
      return this.isAdmin || this.permissions.includes(permission);
    },

    async login(credentials) {
      this.loading = true;
      this.error = null;
      try {
        const result = await authService.login(credentials);
        if (result.success && result.data?.token) this.setAuthData(result.data);
        else if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async register(userData) {
      this.saving = true;
      this.error = null;
      try {
        const result = await authService.register(userData);
        if (result.success && result.data?.token) this.setAuthData(result.data);
        else if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.saving = false;
      }
    },

    setAuthData(data) {
      this.token = data.token;
      const claims = decodeToken(this.token);
      this.roles = claimValues(claims.Role);
      this.permissions = permissionValues(claims.Permission);
      this.user = {
        id: data.userId || claims.UserId || null,
        name: data.userName || claims.UserName || null,
        email: data.email || claims.Email || null,
        entityId: claims.EntityId || null,
        entityType: claims.EntityType || null,
      };
      localStorage.setItem("token", this.token);
      localStorage.setItem("user", JSON.stringify(this.user));
    },

    clearAuth() {
      this.token = null;
      this.user = null;
      this.roles = [];
      this.permissions = [];
      this.error = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },

    logout() {
      this.clearAuth();
    },

    async getPermissions() {
      return authService.getPermissions();
    },

    async getRoles() {
      return authService.getRoles();
    },

    async getAllRolePermissions(roleID) {
      return authService.getAllRolePermissions(roleID);
    },

    async UpdateRolePermissions(changes) {
      return authService.UpdateRolePermissions(changes);
    },
  },
});
