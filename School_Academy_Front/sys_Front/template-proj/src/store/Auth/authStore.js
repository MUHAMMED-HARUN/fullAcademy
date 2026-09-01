import { defineStore } from "pinia";
import authService from "@/Service/Auth/authService";
import { jwtDecode } from "jwt-decode"; // استيراد الدالة

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")) || null,
    token: localStorage.getItem("token") || null,
    permissions: [],
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
    userData: (state) => {
      if (!state.token) return null;
      try {
        const decoded = jwtDecode(state.token);
        // تعيين المفاتيح حسب الأسماء الجديدة في التوكن
        return {
          id: decoded.UserId || decoded.nameid || decoded.sub || null,
          name: decoded.UserName || decoded.unique_name || decoded.name || null,
          email: decoded.Email || decoded.email || null,
          role: decoded.Role || decoded.role || null,
          // الحقول الجديدة المضافة
          entityId: decoded.EntityId || null,
          entityType: decoded.EntityType || null,
          // بيانات إضافية من التوكن
          issuer: decoded.iss || null,
          audience: decoded.aud || null,
          expiresAt: decoded.exp ? new Date(decoded.exp * 1000) : null,
        };
      } catch {
        return null;
      }
    },
    UserRole: (state) => {
      if (!state.token) return null;
      try {
        const decoded = jwtDecode(state.token);
        // تعيين المفاتيح حسب الأسماء الجديدة في التوكن
        return decoded.Role || decoded.role || null;
      } catch {
        return null;
      }
    },
    UserEntityID: (state) => {
      if (!state.token) return null;
      try {
        const decoded = jwtDecode(state.token);
        // تعيين المفاتيح حسب الأسماء الجديدة في التوكن
        return decoded.EntityId || null;
      } catch {
        return null;
      }
    },
  },

  actions: {
    async login(credentials) {
      const result = await authService.login(credentials);

      if (result.success && result.data?.Token) {
        this.setAuthData(result.data);
      }
      return result; // نرجع TResult للواجهة للتعامل مع الأخطاء
    },

    async register(userData) {
      const result = await authService.register(userData);
      if (result.success && result.data?.Token) {
        this.setAuthData(result.data);
      }
      return result;
    },
    hasPermission(permission) {
      return authService.hasPermission(this.currentUser, permission).success;
    },
    async setAuthData(data) {
      this.token = data.Token;
      this.user = {
        id: data.UserId,
        name: data.UserName,
        email: data.Email,
      };
      localStorage.setItem("token", this.token);
      localStorage.setItem("user", JSON.stringify(this.user));
    },

    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },
    async getPermissions() {
      return await authService.getPermissions();
    },
    async getRoles() {
      return await authService.getRoles();
    },
    async getAllRolePermissions(roleID) {
      return await authService.getAllRolePermissions(roleID);
    },
    async UpdateRolePermissions(Changes) {
      return await authService.UpdateRolePermissions(Changes);
    },
  },
});
