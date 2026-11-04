import { defineStore } from "pinia";
import InstituteClass_service from "@/Service/Institute/InstituteClassService";

export const useInstituteClassStore = defineStore("instituteClasses", {
  state: () => ({
    instituteClasses: [],
    selectedInstituteClass: null,
    loading: false,
    error: null,
  }),

  getters: {
    allInstituteClasses: (state) => state.instituteClasses,
    hasInstituteClasses: (state) => state.instituteClasses.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchInstituteClasses() {
      this.loading = true;
      this.error = null;

      const result = await InstituteClass_service.getAll();
      if (result.success) {
        this.instituteClasses = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchInstituteClassBy(command) {
      this.loading = true;
      this.error = null;

      const result = await InstituteClass_service.getBy(command);

      if (result.success) {
        this.selectedInstituteClass = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addInstituteClass(payload) {
      this.loading = true;
      this.error = null;

      const result = await InstituteClass_service.create(payload);
      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchInstituteClasses();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addInstituteClassByRange(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await InstituteClass_service.createByRange(payload);
        if (result.success) {
          await this.fetchInstituteClasses();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateInstituteClass(payload) {
      this.loading = true;
      this.error = null;
      const result = await InstituteClass_service.update(payload);
      if (result.success) {
        await this.fetchInstituteClasses();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteInstituteClass(payload) {
      this.loading = true;
      this.error = null;

      const result = await InstituteClass_service.delete(payload);
      if (result.success) {
        await this.fetchInstituteClasses();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedInstituteClass() {
      this.selectedInstituteClass = null;
    },
  },
});
