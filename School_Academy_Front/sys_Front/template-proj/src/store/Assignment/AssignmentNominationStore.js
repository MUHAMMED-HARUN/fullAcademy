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
      page: 1,
      pageSize: 10,
      totalCount: 0,
      filter: "",
      orderBy: "",
    }),

    getters: {
      allAssignmentNominations: (state) => state.AssignmentNominations,
      hasAssignmentNominations: (state) =>
        state.AssignmentNominations.length > 0,
      isLoading: (state) => state.loading,
    },

    actions: {
      async fetchAssignmentNominations(params = {}) {
        this.loading = true;
        this.error = null;
        if (params.Page !== undefined)
          this.page = Math.max(1, Number(params.Page) || 1);
        if (params.PageSize !== undefined)
          this.pageSize = Math.max(
            1,
            Math.min(100, Number(params.PageSize) || 10)
          );
        if (params.Filter !== undefined) this.filter = params.Filter || "";
        if (params.OrderBy !== undefined) this.orderBy = params.OrderBy || "";
        try {
          const result = await AssignmentNomination_service.getAll({
            Page: this.page,
            PageSize: this.pageSize,
            Filter: this.filter,
            OrderBy: this.orderBy,
          });
          if (result.success) {
            this.AssignmentNominations = result.data?.items || [];
            this.totalCount = result.data?.count || 0;
          } else {
            this.AssignmentNominations = [];
            this.totalCount = 0;
            this.error = result.errors;
          }
          return result;
        } finally {
          this.loading = false;
        }
      },

      async fetchAssignmentNominationtBy(command) {
        this.loading = true;
        this.error = null;
        try {
          const result = await AssignmentNomination_service.getBy(command);
          if (result.success) this.selectedAssignmentNomination = result.data;
          else {
            this.selectedAssignmentNomination = null;
            this.error = result.errors;
          }
          return result;
        } finally {
          this.loading = false;
        }
      },

      async setPage(page) {
        return this.fetchAssignmentNominations({ Page: page });
      },

      async setPageSize(pageSize) {
        return this.fetchAssignmentNominations({ Page: 1, PageSize: pageSize });
      },

      async addAssignmentNomination(payload) {
        this.loading = true;
        this.error = null;

        try {
          const result = await AssignmentNomination_service.create(payload);
          if (result.success)
            await this.fetchAssignmentNominations({ Page: 1 });
          else this.error = result.errors;
          return result;
        } finally {
          this.loading = false;
        }
      },

      async updateAssignmentNomination(payload) {
        this.loading = true;
        this.error = null;

        try {
          const result = await AssignmentNomination_service.update(payload);
          if (result.success) await this.fetchAssignmentNominations();
          else this.error = result.errors;
          return result;
        } finally {
          this.loading = false;
        }
      },

      async DeleteAssignmentNomination(payload) {
        this.loading = true;
        this.error = null;

        try {
          const result = await AssignmentNomination_service.delete(payload);
          if (result.success) await this.fetchAssignmentNominations();
          else this.error = result.errors;
          return result;
        } finally {
          this.loading = false;
        }
      },
      clearSelectedAssignmentNomination() {
        this.selectedAssignmentNomination = null;
      },
    },
  }
);
