import { defineStore } from "pinia";
import daily_evaluation_service from "@/Service/Progress/DailyEvaluationService";

export const useDailyEvaluationStore = defineStore("dailyEvaluation", {
  state: () => ({
    evaluations: [],
    selectedEvaluation: null,
    lastDailyEvaluation: null,
    lastEvaluationLoading: false,
    loading: false,
    error: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    currentFilter: { Filter: "", OrderBy: "" },
    studentAcademicEnrollmentID: "",
    lastEvaluationRequestVersion: 0,
  }),

  getters: {
    allevaluations: (state) => state.evaluations,
    hasEvaluations: (state) => state.evaluations.length > 0,
    isLoading: (state) => state.loading,

    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async GetlastDailyEvaluation(studentAcademicEnrollmentID, stageInfoID) {
      const requestVersion = ++this.lastEvaluationRequestVersion;
      this.lastEvaluationLoading = true;
      this.error = null;
      this.lastDailyEvaluation = null;
      try {
        const result = await daily_evaluation_service.GetlastDailyEvaluation(
          studentAcademicEnrollmentID,
          stageInfoID
        );
        if (requestVersion !== this.lastEvaluationRequestVersion) return result;
        if (result.success) this.lastDailyEvaluation = result.data || null;
        else this.error = result.errors;
        return result;
      } finally {
        if (requestVersion === this.lastEvaluationRequestVersion)
          this.lastEvaluationLoading = false;
      }
    },

    async fetchEvaluations(filter = {}) {
      this.loading = true;
      this.error = null;

      if (filter.Page !== undefined) this.page = filter.Page;
      if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;
      if (
        Object.prototype.hasOwnProperty.call(
          filter,
          "StudentAcademicEnrollmentID"
        )
      ) {
        this.studentAcademicEnrollmentID =
          filter.StudentAcademicEnrollmentID || "";
        this.page = 1;
      }
      const hasFilter = Object.prototype.hasOwnProperty.call(filter, "Filter");
      const hasOrderBy = Object.prototype.hasOwnProperty.call(
        filter,
        "OrderBy"
      );
      if (hasFilter || hasOrderBy) {
        this.currentFilter = {
          Filter: hasFilter ? filter.Filter ?? "" : this.currentFilter.Filter,
          OrderBy: hasOrderBy
            ? filter.OrderBy ?? ""
            : this.currentFilter.OrderBy,
        };
      }

      const queryParams = {
        ...this.currentFilter,
        ...filter,
        StudentAcademicEnrollmentID:
          this.studentAcademicEnrollmentID || undefined,
        Page: this.page,
        PageSize: this.pageSize,
      };
      try {
        const result = await daily_evaluation_service.getAll(queryParams);
        if (result.success) {
          this.evaluations = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.evaluations = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

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

      try {
        const result = await daily_evaluation_service.getBy(command);
        if (result.success) this.selectedEvaluation = result.data;
        else {
          this.selectedEvaluation = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addEvaluation(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await daily_evaluation_service.create(payload);
        if (result.success) await this.fetchEvaluations();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateEvaluation(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await daily_evaluation_service.update(payload);
        if (result.success) await this.fetchEvaluations();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async deleteEvaluation(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await daily_evaluation_service.delete(payload);
        if (result.success) await this.fetchEvaluations();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    clearSelectedEvaluation() {
      this.selectedEvaluation = null;
    },
    clearLastDailyEvaluation() {
      this.lastEvaluationRequestVersion += 1;
      this.lastDailyEvaluation = null;
      this.lastEvaluationLoading = false;
    },
  },
});
