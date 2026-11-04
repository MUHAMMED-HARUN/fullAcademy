import { defineStore } from "pinia";
import Stage_service from "@/Service/Progress/StageService";

export const useStageStore = defineStore("stages", {
  state: () => ({
    stages: [],
    stagePageItems: [],
    selectedStage: null,
    loading: false,
    error: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    currentFilter: { Filter: "", OrderBy: "" },
    previousStageResults: [],
    previousStageLoading: false,
    previousStageError: null,
    previousStageRequestVersion: 0,
  }),

  getters: {
    allStages: (state) => state.stages,
    hasStages: (state) => state.stages.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },

  actions: {
    async fetchStages() {
      this.loading = true;
      this.error = null;
      try {
        const result = await Stage_service.getAll();
        if (result.success) this.stages = result.data || [];
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchStagePage(params = {}) {
      this.loading = true;
      this.error = null;
      try {
        if (params.Page !== undefined)
          this.page = Math.max(1, Number(params.Page) || 1);
        if (params.PageSize !== undefined)
          this.pageSize = Math.max(
            1,
            Math.min(100, Number(params.PageSize) || 10)
          );
        if (
          Object.prototype.hasOwnProperty.call(params, "Filter") ||
          Object.prototype.hasOwnProperty.call(params, "OrderBy")
        ) {
          this.currentFilter = {
            Filter: Object.prototype.hasOwnProperty.call(params, "Filter")
              ? params.Filter ?? ""
              : this.currentFilter.Filter,
            OrderBy: Object.prototype.hasOwnProperty.call(params, "OrderBy")
              ? params.OrderBy ?? ""
              : this.currentFilter.OrderBy,
          };
        }

        const result = await Stage_service.getAllFilter({
          ...this.currentFilter,
          ...params,
          Page: this.page,
          PageSize: this.pageSize,
        });
        if (result.success) {
          this.stagePageItems = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.stagePageItems = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async searchPreviousStages(criteria) {
      const requestVersion = ++this.previousStageRequestVersion;
      this.previousStageLoading = true;
      this.previousStageError = null;
      this.previousStageResults = [];
      try {
        const result = await Stage_service.searchPreviousStages(criteria);
        if (requestVersion !== this.previousStageRequestVersion) return result;
        if (result.success) this.previousStageResults = result.data || [];
        else this.previousStageError = result.errors;
        return result;
      } finally {
        if (requestVersion === this.previousStageRequestVersion)
          this.previousStageLoading = false;
      }
    },

    clearPreviousStageLookup() {
      this.previousStageRequestVersion += 1;
      this.previousStageResults = [];
      this.previousStageError = null;
      this.previousStageLoading = false;
    },

    async setPage(page) {
      return this.fetchStagePage({ Page: page });
    },

    async setPageSize(pageSize) {
      return this.fetchStagePage({ Page: 1, PageSize: pageSize });
    },

    async fetchStagetBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Stage_service.getBy(command);
        if (result.success) this.selectedStage = result.data;
        else {
          this.selectedStage = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addStage(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Stage_service.create(payload);
        if (result.success) {
          await this.fetchStages();
          await this.fetchStagePage();
        } else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateStage(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Stage_service.update(payload);
        if (result.success) {
          await this.fetchStages();
          await this.fetchStagePage();
        } else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeleteStage(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Stage_service.delete(payload);
        if (result.success) {
          await this.fetchStages();
          await this.fetchStagePage();
        } else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    clearSelectedStage() {
      this.selectedStage = null;
    },
  },
});
