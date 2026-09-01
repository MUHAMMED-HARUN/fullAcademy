import { defineStore } from "pinia";
import AssignmentServe from "@/Service/Assignment/AssignmentService";

export const useassignmentstore = defineStore("assignments", {
  state: () => ({
    assignments: [],
    selectedassignments: null,
    loading: false,
    error: null,
  }),

  getters: {
    allassignments: (state) => state.assignments,
    hasassignments: (state) => state.assignments.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchassignments() {
      this.loading = true;
      this.error = null;

      const result = await AssignmentServe.getAll();
      if (result.success) {
        this.assignments = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchStagetBy(command) {
      this.loading = true;
      this.error = null;
      const result = await AssignmentServe.getBy(command);
      if (result.success) {
        this.selectedassignments = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addAssigment(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentServe.create(payload);
      if (result.success) {
        await this.fetchassignments();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateStage(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentServe.update(payload);
      if (result.success) {
        await this.fetchassignments();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteAssignment(payload) {
      this.loading = true;
      this.error = null;

      const result = await AssignmentServe.delete(payload);
      if (result.success) {
        await this.fetchassignments();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearselectedassignments() {
      this.selectedassignments = null;
    },
  },
});
