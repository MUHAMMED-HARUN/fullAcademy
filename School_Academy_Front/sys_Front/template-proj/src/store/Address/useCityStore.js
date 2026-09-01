import { defineStore } from "pinia";
import city_service from "@/Service/Address/city_service";

export const useCityStore = defineStore("city", {
  state: () => ({
    cities: [],
    selectedCity: null,
    loading: false,
    error: null,
  }),

  getters: {
    allCities: (state) => state.cities,
    hasCities: (state) => state.cities.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchCities() {
      this.loading = true;
      this.error = null;

      const result = await city_service.getAll();

      if (result.success) {
        this.cities = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchCityById(param) {
      this.loading = true;
      this.error = null;

      const result = await city_service.getById(param);

      if (result.success) {
        this.selectedCity = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addCity(payload) {
      this.loading = true;
      this.error = null;

      const result = await city_service.create(payload);

      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchCities();
      } else {
        this.error = result.Error;
      }

      this.loading = false;
      return result;
    },

    async updateCity(payload) {
      this.loading = true;
      this.error = null;
      const result = await city_service.update(payload);
      if (result.success) {
        await this.fetchCities();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    clearSelectedCity() {
      this.selectedCity = null;
    },
  },
});
