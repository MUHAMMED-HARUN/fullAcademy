import { defineStore } from "pinia";
import AcademicPlanService from "@/Service/AcademicPlan/AcademicPlanService";

export const useAcademicPlanStore = defineStore("academicPlans", {
  state: () => ({
    academicPlans: [],
    totalCount: 0,
    page: 1,
    pageSize: 10,
    currentFilter: "",
    selectedAcademicPlan: null,
    loading: false,
    error: null,
  }),

  getters: {
    allAcademicPlans: (state) => state.academicPlans,
    hasAcademicPlans: (state) => state.academicPlans.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },

  actions: {
    async fetchAcademicPlans(params = {}) {
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
        if (params.Filter !== undefined)
          this.currentFilter = params.Filter ?? "";

        const result = await AcademicPlanService.getAllFilter({
          Page: this.page,
          PageSize: this.pageSize,
          Filter: this.currentFilter,
        });
        if (result.success) {
          this.academicPlans = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.academicPlans = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async setPage(page) {
      return this.fetchAcademicPlans({ Page: page });
    },

    async setPageSize(pageSize) {
      return this.fetchAcademicPlans({ Page: 1, PageSize: pageSize });
    },

    async fetchAcademicPlanForUpsert(academicPlanID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AcademicPlanService.getForUpsert(academicPlanID);
        if (result.success) this.selectedAcademicPlan = result.data;
        else {
          this.selectedAcademicPlan = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addAcademicPlan(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AcademicPlanService.create(payload);
        if (result.success) await this.fetchAcademicPlans({ Page: 1 });
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateAcademicPlan(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AcademicPlanService.update(payload);
        if (result.success) await this.fetchAcademicPlans();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async activateAcademicPlan(academicPlanID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AcademicPlanService.activate(academicPlanID);
        if (result.success) await this.fetchAcademicPlans();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async deleteAcademicPlan(academicPlanID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AcademicPlanService.delete(academicPlanID);
        if (result.success) await this.fetchAcademicPlans();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    clearSelectedAcademicPlan() {
      this.selectedAcademicPlan = null;
    },
  },
});
