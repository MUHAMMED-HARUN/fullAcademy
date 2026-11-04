import { defineStore } from "pinia";
import CountryService from "@/Service/Address/country_service";

export const useCountryStore = defineStore("country", {
  state: () => ({
    countries: [],
    countryPageItems: [],
    selectedCountry: null,
    loading: false,
    pageLoading: false,
    saving: false,
    error: null,
    pageError: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    filter: { Filter: "", OrderBy: "" },
  }),

  getters: {
    allCountries: (state) => state.countries,
    hasCountries: (state) => state.countries.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchCountries() {
      this.loading = true;
      this.error = null;
      try {
        const result = await CountryService.getAll();
        if (result.success) {
          this.countries = result.data || [];
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchCountryPage(query = {}) {
      if (query.Page !== undefined) this.page = query.Page;
      if (query.PageSize !== undefined) this.pageSize = query.PageSize;
      if (query.Filter !== undefined) this.filter.Filter = query.Filter || "";
      if (query.OrderBy !== undefined)
        this.filter.OrderBy = query.OrderBy || "";

      this.pageLoading = true;
      this.pageError = null;
      try {
        const result = await CountryService.getPage({
          Page: this.page,
          PageSize: this.pageSize,
          ...this.filter,
        });

        if (result.success) {
          this.countryPageItems = result.data?.items || [];
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
      return await this.fetchCountryPage({ Page: page });
    },

    async setPageSize(pageSize) {
      this.page = 1;
      return await this.fetchCountryPage({ Page: 1, PageSize: pageSize });
    },

    async fetchCountryById(param, paramType = 1) {
      this.loading = true;
      this.error = null;
      try {
        const result = await CountryService.getById(param, paramType);
        if (result.success) {
          this.selectedCountry = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addCountry(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await CountryService.create(payload);
        if (result.success) {
          await Promise.all([this.fetchCountries(), this.fetchCountryPage()]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async updateCountry(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await CountryService.update(payload);
        if (result.success) {
          await Promise.all([this.fetchCountries(), this.fetchCountryPage()]);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    clearSelectedCountry() {
      this.selectedCountry = null;
    },
  },
});
