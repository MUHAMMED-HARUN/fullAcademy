import { defineStore } from "pinia";
import daily_evaluation_service from "@/Service/Progress/DailyEvaluationService";

export const useDailyEvaluationStore = defineStore("dailyEvaluation", {
  state: () => ({
    evaluations: [],
    selectedEvaluation: null,
    loading: false,
    error: null,

    // 🔹 حالات التصفح (Paging State)
    page: 1,
    pageSize: 10,
    totalCount: 0,
  }),

  getters: {
    allevaluations: (state) => state.evaluations,
    hasEvaluations: (state) => state.evaluations.length > 0,
    isLoading: (state) => state.loading,

    // 🔹 حساب إجمالي عدد الصفحات
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async GetlastDailyEvaluation(enrollmentId, stageInfoID) {
      this.loading = true;
      this.error = null;

      const result = await daily_evaluation_service.GetlastDailyEvaluation(
        enrollmentId,
        stageInfoID
      );
      if (result.success) {
        this.selectedDailyEvaluation = result.data || null;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchEvaluations(filter = {}) {
      this.loading = true;
      this.error = null;

      // تحديث رقم الصفحة والحجم عند التمرير
      if (filter.Page !== undefined) this.page = filter.Page;
      if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;

      const queryParams = {
        Page: this.page,
        PageSize: this.pageSize,
        ...filter,
      };

      const result = await daily_evaluation_service.getAll(queryParams);
      if (result.success) {
        this.evaluations = result.data?.items || [];
        this.totalCount = result.data?.count || 0;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    // 🔹 التحكم برقم الصفحة
    async setPage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.page = newPage;
        return await this.fetchEvaluations();
      }
    },

    // 🔹 التحكم بحجم الصفحة
    async setPageSize(newSize) {
      this.pageSize = newSize;
      this.page = 1;
      return await this.fetchEvaluations();
    },

    async fetchEvaluationBy(command) {
      this.loading = true;
      this.error = null;

      const result = await daily_evaluation_service.getBy(command);

      if (result.success) {
        this.selectedEvaluation = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addEvaluation(payload) {
      this.loading = true;
      this.error = null;

      const result = await daily_evaluation_service.create(payload);
      if (result.success) {
        await this.fetchEvaluations();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateEvaluation(payload) {
      this.loading = true;
      this.error = null;

      const result = await daily_evaluation_service.update(payload);
      if (result.success) {
        await this.fetchEvaluations();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async deleteEvaluation(payload) {
      this.loading = true;
      this.error = null;

      const result = await daily_evaluation_service.delete(payload);
      if (result.success) {
        await this.fetchEvaluations();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    clearSelectedEvaluation() {
      this.selectedEvaluation = null;
    },
  },
});
