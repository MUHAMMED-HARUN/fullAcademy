import { defineStore } from "pinia";
import neighborhood_service from "@/Service/Address/neighborhood_service";
import TResult from "@/Generic/TResult";

export const useNeighborhoodStore = defineStore("neighborhood", {
  state: () => ({
    neighborhoods: [],
    neighborhoodPageItems: [],
    neighborhoodsByDistrict: [],
    selectedNeighborhood: null,
    loading: false,
    pageLoading: false,
    districtNeighborhoodsLoading: false,
    saving: false,
    error: null,
    pageError: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    filter: { Filter: "", OrderBy: "" },
    neighborhoodSearchResults: [],
    neighborhoodSearchLoading: false,
    neighborhoodSearchError: null,
    neighborhoodSearchRequestId: 0,
  }),

  getters: {
    allNeighborhoods: (state) => state.neighborhoods,
    hasNeighborhoods: (state) => state.neighborhoods.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchNeighborhoods() {
      this.loading = true;
      this.error = null;
      try {
        const result = await neighborhood_service.getAll();
        if (result.success) {
          this.neighborhoods = result.data || [];
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchNeighborhoodPage(query = {}) {
      if (query.Page !== undefined) this.page = query.Page;
      if (query.PageSize !== undefined) this.pageSize = query.PageSize;
      if (query.Filter !== undefined) this.filter.Filter = query.Filter || "";
      if (query.OrderBy !== undefined)
        this.filter.OrderBy = query.OrderBy || "";

      this.pageLoading = true;
      this.pageError = null;
      try {
        const result = await neighborhood_service.getPage({
          Page: this.page,
          PageSize: this.pageSize,
          ...this.filter,
        });

        if (result.success) {
          this.neighborhoodPageItems = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.pageError = result.errors;
        }
        return result;
      } finally {
        this.pageLoading = false;
      }
    },

    async setPage(page) {
      if (page < 1 || page > this.totalPages) return;
      return await this.fetchNeighborhoodPage({ Page: page });
    },

    async setPageSize(pageSize) {
      return await this.fetchNeighborhoodPage({ Page: 1, PageSize: pageSize });
    },

    async fetchNeighborhoodsByDistrictID(districtID, query = {}) {
      this.districtNeighborhoodsLoading = true;
      try {
        const result = await neighborhood_service.getByDistrictId(
          districtID,
          query
        );
        if (result.success) {
          this.neighborhoodsByDistrict = result.data?.items || [];
        }
        return result;
      } finally {
        this.districtNeighborhoodsLoading = false;
      }
    },

    async searchNeighborhoods(criteria) {
      const requestId = ++this.neighborhoodSearchRequestId;
      this.neighborhoodSearchLoading = true;
      this.neighborhoodSearchError = null;
      this.neighborhoodSearchResults = [];

      try {
        const result = await neighborhood_service.searchNeighborhoods(criteria);
        if (requestId !== this.neighborhoodSearchRequestId) return result;

        if (result.success) {
          this.neighborhoodSearchResults = result.data?.items || [];
        } else {
          this.neighborhoodSearchError = result.errors;
        }
        return result;
      } catch (error) {
        const result = new TResult(null, false, {
          errors: error?.message || "تعذر البحث عن الشارع",
        });
        if (requestId === this.neighborhoodSearchRequestId) {
          this.neighborhoodSearchError = result.errors;
        }
        return result;
      } finally {
        if (requestId === this.neighborhoodSearchRequestId) {
          this.neighborhoodSearchLoading = false;
        }
      }
    },

    clearNeighborhoodSearch() {
      this.neighborhoodSearchRequestId += 1;
      this.neighborhoodSearchResults = [];
      this.neighborhoodSearchError = null;
      this.neighborhoodSearchLoading = false;
    },

    async fetchNeighborhoodById(param, paramType = 1) {
      this.loading = true;
      this.error = null;
      try {
        const result = await neighborhood_service.getById(param, paramType);
        if (result.success) {
          this.selectedNeighborhood = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addNeighborhood(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await neighborhood_service.create(payload);
        if (result.success) {
          await Promise.all([
            this.fetchNeighborhoods(),
            this.fetchNeighborhoodPage(),
          ]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async updateNeighborhood(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await neighborhood_service.update(payload);
        if (result.success) {
          await Promise.all([
            this.fetchNeighborhoods(),
            this.fetchNeighborhoodPage(),
          ]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    clearSelectedNeighborhood() {
      this.selectedNeighborhood = null;
    },
  },
});
