import { defineStore } from "pinia";
import Result_service from "@/Service/Assignment/ResultService";

export const useResultStore = defineStore("results", {
  state: () => ({
    Results: [],
    selectedResult: null,
    loading: false,
    error: null,
  }),

  getters: {
    allResults: (state) => state.Results,
    hasResults: (state) => state.Results.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchResults() {
      this.loading = true;
      this.error = null;

      const result = await Result_service.getAll();
      if (result.success) {
        this.Results = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchResulttBy(command) {
      this.loading = true;
      this.error = null;
      const result = await Result_service.getBy(command);
      if (result.success) {
        this.selectedResult = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addResult(payload) {
      this.loading = true;
      this.error = null;

      const result = await Result_service.create(payload);
      if (result.success) {
        await this.fetchResults();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateResult(payload) {
      this.loading = true;
      this.error = null;

      const result = await Result_service.update(payload);
      if (result.success) {
        await this.fetchResults();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteResult(payload) {
      this.loading = true;
      this.error = null;

      const result = await Result_service.delete(payload);
      if (result.success) {
        await this.fetchResults();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedResult() {
      this.selectedResult = null;
    },
  },
});
