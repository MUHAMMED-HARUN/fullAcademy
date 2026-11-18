import { defineStore } from "pinia";
import AssignmentServe from "@/Service/Assignment/AssignmentService";

export const useassignmentstore = defineStore("assignments", {
  state: () => ({
    assignments: [],
    selectedassignments: null,
    loading: false,
    error: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    filter: "",
    orderBy: "",
  }),

  getters: {
    allassignments: (state) => state.assignments,
    hasassignments: (state) => state.assignments.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },

  actions: {
    async fetchassignments(params = {}) {
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
        const result = await AssignmentServe.getAll({
          Page: this.page,
          PageSize: this.pageSize,
          Filter: this.filter,
          OrderBy: this.orderBy,
        });
        if (result.success) {
          this.assignments = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.assignments = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchAssignmentBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AssignmentServe.getBy(command);
        if (result.success) this.selectedassignments = result.data;
        else {
          this.selectedassignments = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchForUpsert(assignmentID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AssignmentServe.getForUpsert(assignmentID);
        this.selectedassignments = result.success ? result.data : null;
        if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addAssignment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AssignmentServe.create(payload);
        if (result.success) await this.fetchassignments({ Page: 1 });
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateAssignment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AssignmentServe.update(payload);
        if (result.success) await this.fetchassignments();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async deleteAssignment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await AssignmentServe.delete(payload);
        if (result.success) await this.fetchassignments();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    async setPage(page) {
      return this.fetchassignments({ Page: page });
    },
    async setPageSize(pageSize) {
      return this.fetchassignments({ Page: 1, PageSize: pageSize });
    },
    clearselectedassignments() {
      this.selectedassignments = null;
    },
  },
});
