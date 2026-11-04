import { defineStore } from "pinia";
import StudentEnrollment_service from "@/Service/Enrollment/StudentEnrollmentService";
import TResult from "@/Generic/TResult";

export const useStudentEnrollmentStore = defineStore("enrollments", {
  state: () => ({
    enrollments: [],
    totalCount: 0,
    page: 1,
    pageSize: 10,
    filter: "",
    orderBy: "",
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
      console.log("enroll", filter);
      // تحديث رقم الصفحة والحجم عند التمرير
      if (filter.Page !== undefined) this.page = filter.Page;
      if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;

      const queryParams = {
        Page: this.page,
        PageSize: this.pageSize,
        ...filter,
      };
      var result = new TResult(null, false, "");
      result = await StudentEnrollment_service.getPagedList(queryParams);

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

      const result = await StudentEnrollment_service.getBy(command);

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

      const result = await StudentEnrollment_service.create(payload);
      if (result.success) {
        await this.fetchEnrollments();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async enrollByRange(payload) {
      this.loading = true;
      this.error = null;

      const result = await StudentEnrollment_service.enrollByRange(payload);
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

      const result = await StudentEnrollment_service.update(payload);
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

      const result = await StudentEnrollment_service.delete(payload);
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
