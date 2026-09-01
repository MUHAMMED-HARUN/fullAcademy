import { defineStore } from "pinia";
import AssignmentScopeinfo_service from "@/Service/Assignment/AssignmentScopeInfoService";

export const useAssignmentScopeinfoStore = defineStore("AssignmentScopeinfos", {
  state: () => ({
    AssignmentScopeinfos: [],
    selectedAssignmentScopeinfo: null,
    loading: false,
    error: null,
  }),

  getters: {
    allAssignmentScopeinfos: (state) => state.AssignmentScopeinfos,
    hasAssignmentScopeinfos: (state) => state.AssignmentScopeinfos.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchAssignmentScopeinfos() {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScopeinfo_service.getAll();
      if (result.success) {
        this.AssignmentScopeinfos = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchAssignmentScopeinfotBy(command) {
      this.loading = true;
      this.error = null;
      const result = await AssignmentScopeinfo_service.getBy(command);
      if (result.success) {
        this.selectedAssignmentScopeinfo = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addAssignmentScopeinfo(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScopeinfo_service.create(payload);
      if (result.success) {
        await this.fetchAssignmentScopeinfos();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateAssignmentScopeinfo(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScopeinfo_service.update(payload);
      if (result.success) {
        await this.fetchAssignmentScopeinfos();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteAssignmentScopeinfo(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScopeinfo_service.delete(payload);
      if (result.success) {
        await this.fetchAssignmentScopeinfos();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedAssignmentScopeinfo() {
      this.selectedAssignmentScopeinfo = null;
    },
  },
});
