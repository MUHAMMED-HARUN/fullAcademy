import { defineStore } from "pinia";
import TeachingAssignment_service from "@/Service/Enrollment/TeachingAssignmentService";
import TResult from "@/Generic/TResult";

export const useTeachingAssignmentStore = defineStore("TeachingAssignment", {
  state: () => ({
    enrollments: [],
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
    async fetchEnrollments(filter = {}, role) {
      this.loading = true;
      this.error = null;

      // تحديث رقم الصفحة والحجم عند التمرير
      if (filter.Page !== undefined) this.page = filter.Page;
      if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;

      const queryParams = {
        Page: this.page,
        PageSize: this.pageSize,
        ...filter,
      };
      var result = new TResult(null, false, "");
      result = await TeachingAssignment_service.getAll(queryParams, role);

      if (result.success) {
        this.enrollments = result.data?.items || [];
        this.totalCount = result.data?.count || 0;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    async fetchEnrollmentBy(command) {
      this.loading = true;
      this.error = null;

      const result = await TeachingAssignment_service.getBy(command);

      if (result.success) {
        this.selectedEnrollment = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addEnrollment(payload) {
      this.loading = true;
      this.error = null;

      const result = await TeachingAssignment_service.create(payload);
      if (result.success) {
        await this.fetchEnrollments();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateEnrollment(payload) {
      this.loading = true;
      this.error = null;

      const result = await TeachingAssignment_service.update(payload);
      if (result.success) {
        await this.fetchEnrollments();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteEnrollment(payload) {
      this.loading = true;
      this.error = null;

      const result = await TeachingAssignment_service.delete(payload);
      if (result.success) {
        await this.fetchEnrollments();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedEnrollment() {
      this.selectedEnrollment = null;
    },
  },
});
