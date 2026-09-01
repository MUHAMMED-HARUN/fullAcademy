import { defineStore } from "pinia";
import StudentStage_service from "@/Service/Progress/StudentStageService";

export const useStudentStageStore = defineStore("StudentStages", {
  state: () => ({
    StudentStages: [],
    selectedStudentStage: null,
    loading: false,
    error: null,
  }),

  getters: {
    allStudentStages: (state) => state.StudentStages,
    hasStudentStages: (state) => state.StudentStages.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchStudentStages() {
      this.loading = true;
      this.error = null;

      const result = await StudentStage_service.getAll();
      if (result.success) {
        this.StudentStages = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchStudentStagetBy(command) {
      this.loading = true;
      this.error = null;
      const result = await StudentStage_service.getBy(command);
      if (result.success) {
        this.selectedStudentStage = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addStudentStage(payload) {
      this.loading = true;
      this.error = null;

      const result = await StudentStage_service.create(payload);
      if (result.success) {
        await this.fetchStudentStages();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateStudentStage(payload) {
      this.loading = true;
      this.error = null;

      const result = await StudentStage_service.update(payload);
      if (result.success) {
        await this.fetchStudentStages();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteStudentStage(payload) {
      this.loading = true;
      this.error = null;

      const result = await StudentStage_service.delete(payload);
      if (result.success) {
        await this.fetchStudentStages();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedStudentStage() {
      this.selectedStudentStage = null;
    },
  },
});
