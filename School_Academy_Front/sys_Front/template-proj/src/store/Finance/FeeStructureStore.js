import { defineStore } from "pinia";
import FeeStructureService from "@/Service/Finance/FeeStructureService";

export const useFeeStructureStore = defineStore("feeStructures", {
  state: () => ({
    feeStructures: [],
    selectedFeeStructure: null,
    loading: false,
    error: null,
  }),

  getters: {
    allFeeStructures: (state) => state.feeStructures,
    hasFeeStructures: (state) => state.feeStructures.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchFeeStructures() {
      this.loading = true;
      this.error = null;
      const result = await FeeStructureService.getAll();
      if (result.success) {
        this.feeStructures = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchFeeStructureBy(command) {
      this.loading = true;
      this.error = null;
      const result = await FeeStructureService.getBy(command);
      if (result.success) {
        this.selectedFeeStructure = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addFeeStructure(payload) {
      this.loading = true;
      this.error = null;
      const result = await FeeStructureService.create(payload);
      if (result.success) {
        await this.fetchFeeStructures();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async updateFeeStructure(payload) {
      this.loading = true;
      this.error = null;
      const result = await FeeStructureService.update(payload);
      if (result.success) {
        await this.fetchFeeStructures();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteFeeStructure(payload) {
      this.loading = true;
      this.error = null;
      const result = await FeeStructureService.delete(payload);
      if (result.success) {
        await this.fetchFeeStructures();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    clearSelectedFeeStructure() {
      this.selectedFeeStructure = null;
    },
  },
});
