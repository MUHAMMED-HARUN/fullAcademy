import { defineStore } from "pinia";
import Class_service from "@/Service/Institute/ClassService";

export const useClassStore = defineStore("classes", {
  state: () => ({
    classes: [],
    selectedClass: null,
    loading: false,
    error: null,
  }),

  getters: {
    allClasses: (state) => state.classes,
    hasClasses: (state) => state.classes.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchClasses() {
      this.loading = true;
      this.error = null;

      const result = await Class_service.getAll();
      if (result.success) {
        this.classes = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchClassBy(command) {
      this.loading = true;
      this.error = null;

      const result = await Class_service.getBy(command);

      if (result.success) {
        this.selectedClass = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addClass(payload) {
      this.loading = true;
      this.error = null;

      const result = await Class_service.create(payload);
      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchClasses();
      } else {
        this.error = result.errors;
      }
      console.log("astore ", result);

      this.loading = false;
      return result;
    },

    async updateClass(payload) {
      this.loading = true;
      this.error = null;
      const result = await Class_service.update(payload);
      if (result.success) {
        await this.fetchClasses();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteClass(payload) {
      this.loading = true;
      this.error = null;

      const result = await Class_service.delete(payload);
      if (result.success) {
        await this.fetchClasses();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedClass() {
      this.selectedClass = null;
    },
  },
});
