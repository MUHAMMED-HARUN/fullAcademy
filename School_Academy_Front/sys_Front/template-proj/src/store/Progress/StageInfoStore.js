import { defineStore } from "pinia";
import StageInfo_service from "@/Service/Progress/StageInfoService";

export const useStageInfoStore = defineStore("stageInfos", {
  state: () => ({
    StageInfos: [],
    stageInfoPageItems: [],
    selectedStageInfo: null,
    loading: false,
    error: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    currentFilter: { Filter: "", OrderBy: "", StageID: "" },
  }),

  getters: {
    allStageInfos: (state) => state.StageInfos,
    hasStageInfos: (state) => state.StageInfos.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },

  actions: {
    async fetchStageInfos() {
      this.loading = true;
      this.error = null;
      try {
        const result = await StageInfo_service.getAll();
        if (result.success) this.StageInfos = result.data || [];
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchStageInfoPage(params = {}) {
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
        const filterKeys = ["Filter", "OrderBy", "StageID"];
        if (
          filterKeys.some((key) =>
            Object.prototype.hasOwnProperty.call(params, key)
          )
        ) {
          this.currentFilter = {
            Filter: Object.prototype.hasOwnProperty.call(params, "Filter")
              ? params.Filter ?? ""
              : this.currentFilter.Filter,
            OrderBy: Object.prototype.hasOwnProperty.call(params, "OrderBy")
              ? params.OrderBy ?? ""
              : this.currentFilter.OrderBy,
            StageID: Object.prototype.hasOwnProperty.call(params, "StageID")
              ? params.StageID ?? ""
              : this.currentFilter.StageID,
          };
        }

        const result = await StageInfo_service.getAllFilter({
          ...this.currentFilter,
          ...params,
          Page: this.page,
          PageSize: this.pageSize,
        });
        if (result.success) {
          this.stageInfoPageItems = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.stageInfoPageItems = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async setPage(page) {
      return this.fetchStageInfoPage({ Page: page });
    },

    async setPageSize(pageSize) {
      return this.fetchStageInfoPage({ Page: 1, PageSize: pageSize });
    },
    async getStudentStageInfo(studentAcademicEnrollmentID) {
      this.loading = true;
      this.error = null;
      this.StageInfos = [];
      try {
        const result = await StageInfo_service.getStudentStageInfo(
          studentAcademicEnrollmentID
        );
        if (result.success) this.StageInfos = result.data || [];
        else {
          this.StageInfos = [];
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },
    async fetchStageInfotBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StageInfo_service.getBy(command);
        if (result.success) this.selectedStageInfo = result.data;
        else {
          this.selectedStageInfo = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addStageInfo(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StageInfo_service.create(payload);
        if (result.success)
          await Promise.all([
            this.fetchStageInfos(),
            this.fetchStageInfoPage(),
          ]);
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateStageInfo(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StageInfo_service.update(payload);
        if (result.success)
          await Promise.all([
            this.fetchStageInfos(),
            this.fetchStageInfoPage(),
          ]);
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeleteStageInfo(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StageInfo_service.delete(payload);
        if (result.success)
          await Promise.all([
            this.fetchStageInfos(),
            this.fetchStageInfoPage(),
          ]);
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    clearSelectedStageInfo() {
      this.selectedStageInfo = null;
    },
  },
});
