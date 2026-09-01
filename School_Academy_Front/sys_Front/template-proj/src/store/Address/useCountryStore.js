import { defineStore } from "pinia";
import CountryService from "@/Service/Address/country_service";

export const useCountryStore = defineStore("country", {
  state: () => ({
    countries: [],
    selectedCountry: null,
    loading: false,
    error: null,
  }),

  getters: {
    allCountries: (state) => state.countries,
    hasCountries: (state) => state.countries.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchCountries() {
      this.loading = true;
      this.error = null;

      const result = await CountryService.getAll();

      if (result.success) {
        this.countries = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchCountryById(param, paramType = 1) {
      this.loading = true;
      this.error = null;

      const result = await CountryService.getById(param, paramType);

      if (result.success) {
        this.selectedCountry = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addCountry(payload) {
      this.loading = true;
      this.error = null;

      const result = await CountryService.create(payload);

      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchCountries();
      } else {
        this.error = result.Error;
      }

      this.loading = false;
      return result;
    },

    async updateCountry(payload) {
      this.loading = true;
      this.error = null;
      const result = await CountryService.update(payload);
      if (result.success) {
        await this.fetchCountries();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    clearSelectedCountry() {
      this.selectedCountry = null;
    },
  },
});
