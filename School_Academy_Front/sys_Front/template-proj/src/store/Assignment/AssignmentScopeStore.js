import { defineStore } from "pinia";
import AssignmentScope_service from "@/Service/Assignment/AssignmentScopeService";

export const useAssignmentScopeStore = defineStore("AssignmentScopes", {
  state: () => ({
    AssignmentScopes: [],
    selectedAssignmentScope: null,
    loading: false,
    error: null,
  }),

  getters: {
    allAssignmentScopes: (state) => state.AssignmentScopes,
    hasAssignmentScopes: (state) => state.AssignmentScopes.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchAssignmentScopes() {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScope_service.getAll();
      if (result.success) {
        this.AssignmentScopes = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchAssignmentScopetBy(command) {
      this.loading = true;
      this.error = null;
      const result = await AssignmentScope_service.getBy(command);
      if (result.success) {
        this.selectedAssignmentScope = result.data;
      } else {
        this.error = result.errors;
      }
      console.log("sdfgsfdgjrsfd", this.selectedAssignmentScope);
      this.loading = false;
      return result;
    },
    async fetchCanNominateList(command) {
      this.loading = true;
      this.error = null;
      const result = await AssignmentScope_service.GetCanNominateList(command);
      if (result.success) {
        this.AssignmentScopes = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addAssignmentScope(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScope_service.create(payload);
      if (result.success) {
        await this.fetchAssignmentScopes();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateAssignmentScope(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScope_service.update(payload);
      if (result.success) {
        await this.fetchAssignmentScopes();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteAssignmentScope(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentScope_service.delete(payload);
      if (result.success) {
        await this.fetchAssignmentScopes();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedAssignmentScope() {
      this.selectedAssignmentScope = null;
    },
  },
});
