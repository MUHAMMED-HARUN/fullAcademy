import { defineStore } from "pinia";
import FeeStructureService from "@/Service/Finance/FeeStructureService";

export const useFeeStructureStore = defineStore("feeStructures", {
  state: () => ({
    feeStructures: [],
    totalCount: 0,
    page: 1,
    pageSize: 10,
    currentFilter: "",
    currentOrderBy: "",
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
      return this.fetchFeeStructurePage();
    },

    async fetchFeeStructurePage(params = {}) {
      this.loading = true;
      this.error = null;
      try {
        const result = await FeeStructureService.getAllFilter({
          Page: params.Page ?? this.page,
          PageSize: params.PageSize ?? this.pageSize,
          Filter: params.Filter ?? this.currentFilter,
          OrderBy: params.OrderBy ?? this.currentOrderBy,
        });
        if (result.success) {
          this.feeStructures = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
          this.page = params.Page ?? this.page;
          this.pageSize = params.PageSize ?? this.pageSize;
          if (params.Filter !== undefined) this.currentFilter = params.Filter;
          if (params.OrderBy !== undefined)
            this.currentOrderBy = params.OrderBy;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchByAcademicPlan(academicPlanID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await FeeStructureService.getByAcademicPlan(
          academicPlanID
        );
        if (result.success) this.feeStructures = result.data || [];
        else {
          this.feeStructures = [];
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
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
