import { defineStore } from "pinia";
import District_service from "@/Service/Address/District_service";
import TResult from "@/Generic/TResult";

export const useDistrictStore = defineStore("districts", {
  state: () => ({
    districts: [],
    districtPageItems: [],
    districtsByCity: [],
    selecteddistricts: null,
    loading: false,
    pageLoading: false,
    cityDistrictsLoading: false,
    saving: false,
    error: null,
    pageError: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    filter: { Filter: "", OrderBy: "" },
    districtSearchResults: [],
    districtSearchLoading: false,
    districtSearchError: null,
    districtSearchRequestId: 0,
  }),

  getters: {
    allDistricts: (state) => state.districts,
    hasDistricts: (state) => state.districts.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchDistricts() {
      this.loading = true;
      this.error = null;
      try {
        const result = await District_service.getAll();
        if (result.success) {
          this.districts = result.data || [];
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchDistrictPage(query = {}) {
      if (query.Page !== undefined) this.page = query.Page;
      if (query.PageSize !== undefined) this.pageSize = query.PageSize;
      if (query.Filter !== undefined) this.filter.Filter = query.Filter || "";
      if (query.OrderBy !== undefined)
        this.filter.OrderBy = query.OrderBy || "";

      this.pageLoading = true;
      this.pageError = null;
      try {
        const result = await District_service.getPage({
          Page: this.page,
          PageSize: this.pageSize,
          ...this.filter,
        });

        if (result.success) {
          this.districtPageItems = result.data?.items || [];
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
      return await this.fetchDistrictPage({ Page: page });
    },

    async setPageSize(pageSize) {
      return await this.fetchDistrictPage({ Page: 1, PageSize: pageSize });
    },

    async fetchDistrictsByCityID(cityID, query = {}) {
      this.cityDistrictsLoading = true;
      try {
        const result = await District_service.getByCityId(cityID, query);
        if (result.success) this.districtsByCity = result.data?.items || [];
        return result;
      } finally {
        this.cityDistrictsLoading = false;
      }
    },

    async searchDistricts(criteria) {
      const requestId = ++this.districtSearchRequestId;
      this.districtSearchLoading = true;
      this.districtSearchError = null;
      this.districtSearchResults = [];

      try {
        const result = await District_service.searchDistricts(criteria);
        if (requestId !== this.districtSearchRequestId) return result;

        if (result.success) {
          this.districtSearchResults = result.data?.items || [];
        } else {
          this.districtSearchError = result.errors;
        }
        return result;
      } catch (error) {
        const result = new TResult(null, false, {
          errors: error?.message || "تعذر البحث عن المنطقة",
        });
        if (requestId === this.districtSearchRequestId) {
          this.districtSearchError = result.errors;
        }
        return result;
      } finally {
        if (requestId === this.districtSearchRequestId) {
          this.districtSearchLoading = false;
        }
      }
    },

    clearDistrictSearch() {
      this.districtSearchRequestId += 1;
      this.districtSearchResults = [];
      this.districtSearchError = null;
      this.districtSearchLoading = false;
    },

    async fetchDistrictById(param, paramType = 1) {
      this.loading = true;
      this.error = null;
      try {
        const result = await District_service.getById(param, paramType);
        if (result.success) {
          this.selecteddistricts = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addDistrict(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await District_service.create(payload);
        if (result.success) {
          await Promise.all([this.fetchDistricts(), this.fetchDistrictPage()]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async updateDistrict(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await District_service.update(payload);
        if (result.success) {
          await Promise.all([this.fetchDistricts(), this.fetchDistrictPage()]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    clearSelectedDistrict() {
      this.selecteddistricts = null;
    },
  },
});
