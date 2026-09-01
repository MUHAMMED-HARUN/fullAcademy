import { defineStore } from "pinia";
import neighborhood_service from "@/Service/Address/neighborhood_service";

export const useNeighborhoodStore = defineStore("neighborhood", {
  state: () => ({
    neighborhoods: [],
    selectedNeighborhood: null,
    loading: false,
    error: null,
  }),

  getters: {
    allNeighborhoods: (state) => state.districts,
    hasNeighborhoods: (state) => state.districts.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchNeighborhoods() {
      this.loading = true;
      this.error = null;

      const result = await neighborhood_service.getAll();

      if (result.success) {
        this.neighborhoods = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchNeighborhoodById(param) {
      this.loading = true;
      this.error = null;

      const result = await neighborhood_service.getById(param);

      if (result.success) {
        this.selectedNeighborhood = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addNeighborhood(payload) {
      this.loading = true;
      this.error = null;

      const result = await neighborhood_service.create(payload);

      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchNeighborhoods();
      } else {
        this.error = result.Error;
      }

      this.loading = false;
      return result;
    },

    async updateNeighborhood(payload) {
      this.loading = true;
      this.error = null;
      const result = await neighborhood_service.update(payload);
      if (result.success) {
        await this.fetchNeighborhoods();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    clearSelectedNeighborhood() {
      this.selectedNeighborhood = null;
    },
  },
});
