import { defineStore } from "pinia";
import StageInfo_service from "@/Service/Progress/StageInfoService";

export const useStageInfoStore = defineStore("stageInfos", {
  state: () => ({
    StageInfos: [],
    selectedStageInfo: null,
    loading: false,
    error: null,
  }),

  getters: {
    allStageInfos: (state) => state.StageInfos,
    hasStageInfos: (state) => state.StageInfos.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchStageInfos() {
      this.loading = true;
      this.error = null;

      const result = await StageInfo_service.getAll();
      if (result.success) {
        this.StageInfos = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    async getStudentStageInfo(enrollmentID) {
      this.loading = true;
      this.error = null;

      const result = await StageInfo_service.getStudentStageInfo(enrollmentID);
      console.log(result);
      if (result.success) {
        this.StageInfos = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },
    async fetchStageInfotBy(command) {
      this.loading = true;
      this.error = null;

      const result = await StageInfo_service.getBy(command);

      if (result.success) {
        this.StageInfos = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addStageInfo(payload) {
      this.loading = true;
      this.error = null;

      const result = await StageInfo_service.create(payload);
      if (result.success) {
        await this.fetchStageInfos();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateStageInfo(payload) {
      this.loading = true;
      this.error = null;

      const result = await StageInfo_service.update(payload);
      if (result.success) {
        await this.fetchStageInfos();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteStageInfo(payload) {
      this.loading = true;
      this.error = null;

      const result = await StageInfo_service.delete(payload);
      if (result.success) {
        await this.fetchStageInfos();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedStageInfo() {
      this.selectedStageInfo = null;
    },
  },
});
