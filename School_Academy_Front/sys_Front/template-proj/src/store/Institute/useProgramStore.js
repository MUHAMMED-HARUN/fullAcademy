import { defineStore } from "pinia";
import program_repositories from "@/Service/Institute/ProgramService";

import ProgramMapper from "@/Mapper/Institute/ProgramMapper";

export const useProgramStore = defineStore("programs", {
  state: () => ({
    programs: [],
    selectedProgram: null,
    loading: false,
    error: null,
  }),

  getters: {
    allprograms: (state) => state.programs,
    hasPrograms: (state) => state.programs.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchPrograms() {
      this.loading = true;
      this.error = null;

      const result = await program_repositories.getAll();
      if (result.success) {
        this.programs = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchProgrameBy(command) {
      this.loading = true;
      this.error = null;

      const result = await program_repositories.getBy(command);

      if (result.success) {
        this.selectedProgram = ProgramMapper.MapFromApiDTO(result.data);
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addProgram(payload) {
      this.loading = true;
      this.error = null;

      const result = await program_repositories.create(payload);
      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchPrograms();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateProgram(payload) {
      this.loading = true;
      this.error = null;
      const result = await program_repositories.update(payload);
      if (result.success) {
        await this.fetchPrograms();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteProgram(payload) {
      this.loading = true;
      this.error = null;

      const result = await program_repositories.delete(payload);
      if (result.success) {
        await this.fetchPrograms();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedProgram() {
      this.selectedInstitute = null;
    },
  },
});
