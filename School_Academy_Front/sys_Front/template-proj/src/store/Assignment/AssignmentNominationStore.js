import { defineStore } from "pinia";
import AssignmentNomination_service from "@/Service/Assignment/AssignmentNominationService";

export const useAssignmentNominationStore = defineStore(
  "assignmentNominations",
  {
    state: () => ({
      AssignmentNominations: [],
      selectedAssignmentNomination: null,
      loading: false,
      error: null,
    }),

    getters: {
      allAssignmentNominations: (state) => state.AssignmentNominations,
      hasAssignmentNominations: (state) =>
        state.AssignmentNominations.length > 0,
      isLoading: (state) => state.loading,
    },

    actions: {
      async fetchAssignmentNominations() {
        this.loading = true;
        this.error = null;

        const result = await AssignmentNomination_service.getAll();
        if (result.success) {
          this.AssignmentNominations = result.data || [];
        } else {
          this.error = result.errors;
        }
        this.loading = false;
        return result;
      },

      async fetchAssignmentNominationtBy(command) {
        this.loading = true;
        this.error = null;
        const result = await AssignmentNomination_service.getBy(command);
        if (result.success) {
          this.selectedAssignmentNomination = result.data;
        } else {
          this.error = result.errors;
        }
        this.loading = false;
        return result;
      },

      async addAssignmentNomination(payload) {
        this.loading = true;
        this.error = null;

        const result = await AssignmentNomination_service.create(payload);
        if (result.success) {
          await this.fetchAssignmentNominations();
        } else {
          this.error = result.errors;
        }

        this.loading = false;
        return result;
      },

      async updateAssignmentNomination(payload) {
        this.loading = true;
        this.error = null;

        const result = await AssignmentNomination_service.update(payload);
        if (result.success) {
          await this.fetchAssignmentNominations();
        } else {
          this.error = result.errors;
        }
        this.loading = false;
        return result;
      },

      async DeleteAssignmentNomination(payload) {
        this.loading = true;
        this.error = null;

        const result = await AssignmentNomination_service.delete(payload);
        if (result.success) {
          await this.fetchAssignmentNominations();
        } else {
          this.error = result.errors;
        }
        this.loading = false;
        return result;
      },
      clearSelectedAssignmentNomination() {
        this.selectedAssignmentNomination = null;
      },
    },
  }
);
