import { defineStore } from "pinia";
import institute_service from "@/Service/Institute/InstituteService";
import InstituteMapper from "@/Mapper/Institute/InstituteMapper";

export const useInstituteStore = defineStore("institute", {
  state: () => ({
    institutes: [],
    selectedInstitute: null,
    instituteLookupResults: [],
    instituteLookupLoading: false,
    instituteLookupError: null,
    instituteLookupRequestId: 0,
    loading: false,
    error: null,
  }),

  getters: {
    allinstitutes: (state) => state.institutes,
    hasInstitutes: (state) => state.institutes.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchInstitutes() {
      this.loading = true;
      this.error = null;

      const result = await institute_service.getAll();
      if (result.success) {
        this.institutes = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchInstituteBy(command) {
      this.loading = true;
      this.error = null;

      const result = await institute_service.getBy(command);

      if (result.success) {
        this.selectedInstitute = InstituteMapper.MapFromApiDTO(result.data);
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async searchInstitutes(criteria) {
      const requestId = ++this.instituteLookupRequestId;
      this.instituteLookupLoading = true;
      this.instituteLookupError = null;
      this.instituteLookupResults = [];

      try {
        const result = await institute_service.searchInstitutes(criteria);
        if (requestId !== this.instituteLookupRequestId) return result;

        if (result.success) {
          this.instituteLookupResults = result.data?.items || [];
        } else {
          this.instituteLookupError = result.errors;
        }
        return result;
      } finally {
        if (requestId === this.instituteLookupRequestId) {
          this.instituteLookupLoading = false;
        }
      }
    },

    clearInstituteLookup() {
      this.instituteLookupRequestId += 1;
      this.instituteLookupResults = [];
      this.instituteLookupLoading = false;
      this.instituteLookupError = null;
    },

    async addInstitute(payload) {
      this.loading = true;
      this.error = null;

      const result = await institute_service.create(payload);
      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchInstitutes();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateInstitute(payload) {
      this.loading = true;
      this.error = null;
      const result = await institute_service.update(payload);
      if (result.success) {
        await this.fetchInstitutes();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteInstitute(payload) {
      this.loading = true;
      this.error = null;

      const result = await institute_service.delete(payload);
      if (result.success) {
        await this.fetchInstitutes();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedInstitute() {
      this.selectedInstitute = null;
    },
  },
});
