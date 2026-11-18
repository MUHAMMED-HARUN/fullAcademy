import { defineStore } from "pinia";
import Result_service from "@/Service/Assignment/ResultService";

export const useResultStore = defineStore("results", {
  state: () => ({
    Results: [],
    selectedResult: null,
    loading: false,
    error: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    filter: "",
    orderBy: "",
    assignmentID: "",
  }),

  getters: {
    allResults: (state) => state.Results,
    hasResults: (state) => state.Results.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },

  actions: {
    async fetchResults(params = {}) {
      this.loading = true;
      this.error = null;
      if (params.Page !== undefined)
        this.page = Math.max(1, Number(params.Page) || 1);
      if (params.PageSize !== undefined)
        this.pageSize = Math.max(
          1,
          Math.min(100, Number(params.PageSize) || 10)
        );
      if (Object.hasOwn(params, "AssignmentID")) {
        this.assignmentID = params.AssignmentID || "";
        this.page = 1;
      }
      if (params.Filter !== undefined) this.filter = params.Filter || "";
      if (params.OrderBy !== undefined) this.orderBy = params.OrderBy || "";
      try {
        const result = await Result_service.getAll({
          Page: this.page,
          PageSize: this.pageSize,
          AssignmentID: this.assignmentID || undefined,
          Filter: this.filter,
          OrderBy: this.orderBy,
        });
        if (result.success) {
          this.Results = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.Results = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchResulttBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Result_service.getBy(command);
        if (result.success) this.selectedResult = result.data;
        else {
          this.selectedResult = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchForUpsert(resultID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Result_service.getForUpsert(resultID);
        this.selectedResult = result.success ? result.data : null;
        if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addResult(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await Result_service.create(payload);
        if (result.success) await this.fetchResults();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateResult(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await Result_service.update(payload);
        if (result.success) await this.fetchResults();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeleteResult(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await Result_service.delete(payload);
        if (result.success) await this.fetchResults();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async setPage(page) {
      return this.fetchResults({ Page: page });
    },

    async setPageSize(pageSize) {
      return this.fetchResults({ Page: 1, PageSize: pageSize });
    },
    clearSelectedResult() {
      this.selectedResult = null;
    },
  },
});
