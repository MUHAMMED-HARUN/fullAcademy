import { defineStore } from "pinia";
import InstituteClass_service from "@/Service/Institute/InstituteClassService";

export const useInstituteClassStore = defineStore("instituteClasses", {
  state: () => ({
    instituteClasses: [],
    instituteClassLookupResults: [],
    instituteClassLookupCount: 0,
    instituteClassLookupLoading: false,
    instituteClassLookupError: null,
    instituteClassLookupRequestID: 0,
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
    async fetchInstituteClassLookup(params = {}) {
      const requestID = ++this.instituteClassLookupRequestID;
      this.instituteClassLookupLoading = true;
      this.instituteClassLookupError = null;
      try {
        const result = await InstituteClass_service.getAllFilter({
          Page: Math.max(1, Number(params.Page) || 1),
          PageSize: Math.max(1, Math.min(100, Number(params.PageSize) || 20)),
          Filter: params.Filter || "",
        });
        if (requestID !== this.instituteClassLookupRequestID) return result;
        this.instituteClassLookupResults = result.success
          ? result.data?.items || []
          : [];
        this.instituteClassLookupCount = result.success
          ? result.data?.count || 0
          : 0;
        if (!result.success) this.instituteClassLookupError = result.errors;
        return result;
      } finally {
        if (requestID === this.instituteClassLookupRequestID)
          this.instituteClassLookupLoading = false;
      }
    },

    clearInstituteClassLookup() {
      this.instituteClassLookupRequestID += 1;
      this.instituteClassLookupResults = [];
      this.instituteClassLookupCount = 0;
      this.instituteClassLookupError = null;
      this.instituteClassLookupLoading = false;
    },

    buildLookupFilter(type, value) {
      return InstituteClass_service.buildLookupFilter(type, value);
    },

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
      this.selectedInstituteClass = null;

      try {
        const result = await InstituteClass_service.getBy(command);
        if (result.success) {
          this.selectedInstituteClass = result.data;
        } else {
          this.selectedInstituteClass = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
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
