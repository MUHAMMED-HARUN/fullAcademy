import { defineStore } from "pinia";
import StudentEnrollment_service from "@/Service/Enrollment/StudentEnrollmentService";
export const useStudentEnrollmentStore = defineStore("enrollments", {
  state: () => ({
    enrollments: [],
    totalCount: 0,
    page: 1,
    pageSize: 10,
    currentFilter: { Filter: "", OrderBy: "" },
    selectedEnrollment: null,
    loading: false,
    error: null,
  }),

  getters: {
    allEnrollments: (state) => state.enrollments,
    hasEnrollments: (state) => state.enrollments.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchEnrollments(filter = {}) {
      this.loading = true;
      this.error = null;

      // Normalize incoming keys to the PascalCase shape expected by the API
      const normalizedFilter = { ...filter };
      if (normalizedFilter.page !== undefined && normalizedFilter.Page === undefined)
        normalizedFilter.Page = normalizedFilter.page;
      if (normalizedFilter.pageSize !== undefined && normalizedFilter.PageSize === undefined)
        normalizedFilter.PageSize = normalizedFilter.pageSize;
      if (normalizedFilter.filter !== undefined && normalizedFilter.Filter === undefined)
        normalizedFilter.Filter = normalizedFilter.filter;
      if (normalizedFilter.orderBy !== undefined && normalizedFilter.OrderBy === undefined)
        normalizedFilter.OrderBy = normalizedFilter.orderBy;

      if (normalizedFilter.Page !== undefined)
        this.page = Math.max(1, Number(normalizedFilter.Page) || 1);
      if (normalizedFilter.PageSize !== undefined)
        this.pageSize = Math.max(
          1,
          Math.min(100, Number(normalizedFilter.PageSize) || 10)
        );

      const hasFilter = Object.prototype.hasOwnProperty.call(normalizedFilter, "Filter");
      const hasOrderBy = Object.prototype.hasOwnProperty.call(
        normalizedFilter,
        "OrderBy"
      );
      if (hasFilter || hasOrderBy) {
        this.currentFilter = {
          Filter: hasFilter ? normalizedFilter.Filter ?? "" : this.currentFilter.Filter,
          OrderBy: hasOrderBy
            ? normalizedFilter.OrderBy ?? ""
            : this.currentFilter.OrderBy,
        };
      }

      const queryParams = {
        ...this.currentFilter,
        Page: this.page,
        PageSize: this.pageSize,
        Filter: this.currentFilter.Filter,
        OrderBy: this.currentFilter.OrderBy,
      };

      try {
        const result = await StudentEnrollment_service.getPagedList(
          queryParams
        );
        if (result.success) {
          this.enrollments = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.enrollments = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchEnrollmentBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentEnrollment_service.getBy(command);
        if (result.success) this.selectedEnrollment = result.data;
        else {
          this.selectedEnrollment = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addEnrollment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentEnrollment_service.create(payload);
        if (result.success) await this.fetchEnrollments();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async enrollByRange(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentEnrollment_service.enrollByRange(payload);
        if (result.success) await this.fetchEnrollments();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateEnrollment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentEnrollment_service.update(payload);
        if (result.success) await this.fetchEnrollments();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeleteEnrollment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentEnrollment_service.delete(payload);
        if (result.success) await this.fetchEnrollments();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    clearSelectedEnrollment() {
      this.selectedEnrollment = null;
    },
  },
});
