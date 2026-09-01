import { defineStore } from "pinia";
import District_service from "@/Service/Address/District_service";

export const useDistrictStore = defineStore("districts", {
  state: () => ({
    districts: [],
    selecteddistricts: null,
    loading: false,
    error: null,
  }),

  getters: {
    allDistricts: (state) => state.districts,
    hasDistricts: (state) => state.districts.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchDistricts() {
      this.loading = true;
      this.error = null;

      const result = await District_service.getAll();

      if (result.success) {
        this.districts = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchCityById(param) {
      this.loading = true;
      this.error = null;

      const result = await District_service.getById(param);

      if (result.success) {
        this.selecteddistricts = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addDistrict(payload) {
      this.loading = true;
      this.error = null;

      const result = await District_service.create(payload);

      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchDistricts();
      } else {
        this.error = result.Error;
      }

      this.loading = false;
      return result;
    },

    async updateDistrict(payload) {
      this.loading = true;
      this.error = null;
      const result = await District_service.update(payload);
      if (result.success) {
        await this.fetchDistricts();
      } else {
        this.error = result.errors;
      }
      console.log(result);
      this.loading = false;
      return result;
    },

    clearSelectedDistrict() {
      this.selecteddistricts = null;
    },
  },
});
