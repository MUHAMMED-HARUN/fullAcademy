import { defineStore } from "pinia";
import Stage_service from "@/Service/Progress/StageService";

export const useStageStore = defineStore("stages", {
  state: () => ({
    stages: [],
    selectedStage: null,
    loading: false,
    error: null,
  }),

  getters: {
    allStages: (state) => state.stages,
    hasStages: (state) => state.stages.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchStages() {
      this.loading = true;
      this.error = null;

      const result = await Stage_service.getAll();
      if (result.success) {
        this.stages = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchStagetBy(command) {
      this.loading = true;
      this.error = null;
      const result = await Stage_service.getBy(command);
      if (result.success) {
        this.selectedStage = result.data;
      } else {
        this.error = result.errors;
      }
      console.log("sdfgsfdgjrsfd", this.selectedStage);
      this.loading = false;
      return result;
    },

    async addStage(payload) {
      this.loading = true;
      this.error = null;

      const result = await Stage_service.create(payload);
      if (result.success) {
        await this.fetchStages();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateStage(payload) {
      this.loading = true;
      this.error = null;

      const result = await Stage_service.update(payload);
      if (result.success) {
        await this.fetchStages();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteStage(payload) {
      this.loading = true;
      this.error = null;

      const result = await Stage_service.delete(payload);
      if (result.success) {
        await this.fetchStages();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedStage() {
      this.selectedStage = null;
    },
  },
});
