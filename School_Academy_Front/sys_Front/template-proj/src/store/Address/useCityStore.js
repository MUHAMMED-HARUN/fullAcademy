import { defineStore } from "pinia";
import city_service from "@/Service/Address/city_service";
import TResult from "@/Generic/TResult";

export const useCityStore = defineStore("city", {
  state: () => ({
    cities: [],
    cityPageItems: [],
    selectedCity: null,
    loading: false,
    pageLoading: false,
    saving: false,
    error: null,
    pageError: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    filter: { Filter: "", OrderBy: "" },
    citySearchResults: [],
    citySearchLoading: false,
    citySearchError: null,
    citySearchRequestId: 0,
    citySearchLookups: {},
  }),

  getters: {
    allCities: (state) => state.cities,
    hasCities: (state) => state.cities.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
    cityLookup: (state) => (key) =>
      state.citySearchLookups[key] || {
        results: [],
        loading: false,
        error: null,
        requestId: 0,
      },
  },

  actions: {
    async fetchCities() {
      this.loading = true;
      this.error = null;
      try {
        const result = await city_service.getAll();
        if (result.success) {
          this.cities = result.data || [];
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchCityPage(query = {}) {
      if (query.Page !== undefined) this.page = query.Page;
      if (query.PageSize !== undefined) this.pageSize = query.PageSize;
      if (query.Filter !== undefined) this.filter.Filter = query.Filter || "";
      if (query.OrderBy !== undefined)
        this.filter.OrderBy = query.OrderBy || "";

      this.pageLoading = true;
      this.pageError = null;
      try {
        const result = await city_service.getPage({
          Page: this.page,
          PageSize: this.pageSize,
          ...this.filter,
        });

        if (result.success) {
          this.cityPageItems = result.data?.items || [];
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
      return await this.fetchCityPage({ Page: page });
    },

    async setPageSize(pageSize) {
      return await this.fetchCityPage({ Page: 1, PageSize: pageSize });
    },

    async searchCities(criteria) {
      const requestId = ++this.citySearchRequestId;
      this.citySearchLoading = true;
      this.citySearchError = null;
      this.citySearchResults = [];

      try {
        const result = await city_service.searchCities(criteria);
        if (requestId !== this.citySearchRequestId) return result;

        if (result.success) {
          this.citySearchResults = result.data?.items || [];
        } else {
          this.citySearchError = result.errors;
        }
        return result;
      } catch (error) {
        const result = new TResult(null, false, {
          errors: error?.message || "تعذر البحث عن المدينة",
        });
        if (requestId === this.citySearchRequestId) {
          this.citySearchError = result.errors;
        }
        return result;
      } finally {
        if (requestId === this.citySearchRequestId) {
          this.citySearchLoading = false;
        }
      }
    },

    async searchCitiesFor(criteria, lookupKey) {
      const current = this.citySearchLookups[lookupKey];
      const requestId = (current?.requestId || 0) + 1;
      this.citySearchLookups[lookupKey] = {
        requestId,
        results: [],
        loading: true,
        error: null,
      };

      try {
        const result = await city_service.searchCities(criteria);
        if (this.citySearchLookups[lookupKey]?.requestId !== requestId)
          return result;

        this.citySearchLookups[lookupKey] = {
          requestId,
          results: result.success ? result.data?.items || [] : [],
          loading: false,
          error: result.success ? null : result.errors,
        };
        return result;
      } catch (error) {
        const result = new TResult(null, false, {
          errors: error?.message || "تعذر البحث عن المدينة",
        });
        if (this.citySearchLookups[lookupKey]?.requestId === requestId) {
          this.citySearchLookups[lookupKey] = {
            requestId,
            results: [],
            loading: false,
            error: result.errors,
          };
        }
        return result;
      }
    },

    clearCitySearchFor(lookupKey) {
      const requestId = (this.citySearchLookups[lookupKey]?.requestId || 0) + 1;
      this.citySearchLookups[lookupKey] = {
        requestId,
        results: [],
        loading: false,
        error: null,
      };
    },

    clearCitySearch() {
      this.citySearchRequestId += 1;
      this.citySearchResults = [];
      this.citySearchError = null;
      this.citySearchLoading = false;
    },

    async fetchCityById(param, paramType = 1) {
      this.loading = true;
      this.error = null;
      try {
        const result = await city_service.getById(param, paramType);
        if (result.success) {
          this.selectedCity = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addCity(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await city_service.create(payload);
        if (result.success) {
          await Promise.all([this.fetchCities(), this.fetchCityPage()]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async updateCity(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await city_service.update(payload);
        if (result.success) {
          await Promise.all([this.fetchCities(), this.fetchCityPage()]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    clearSelectedCity() {
      this.selectedCity = null;
    },
  },
});
