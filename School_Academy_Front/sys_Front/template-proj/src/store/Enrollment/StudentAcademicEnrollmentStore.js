import { defineStore } from "pinia";
import StudentAcademicEnrollmentService from "@/Service/Enrollment/StudentAcademicEnrollmentService";

export const useStudentAcademicEnrollmentStore = defineStore(
  "studentAcademicEnrollments",
  {
    state: () => ({
      enrollments: [],
      totalCount: 0,
      page: 1,
      pageSize: 10,
      filter: "",
      groupID: "",
      activeOnly: false,
      selectedEnrollment: null,
      loading: false,
      error: null,
    }),
    getters: {
      totalPages: (state) =>
        Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
    },
    actions: {
      async fetchEnrollments(params = {}) {
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
        if (params.GroupID !== undefined) this.groupID = params.GroupID || "";
        if (params.ActiveOnly !== undefined)
          this.activeOnly = Boolean(params.ActiveOnly);
        try {
          const result = await StudentAcademicEnrollmentService.getListFilter({
            Page: this.page,
            PageSize: this.pageSize,
            Filter: this.filter,
            GroupID: this.groupID || undefined,
            ActiveOnly: this.activeOnly,
          });
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
      async enroll(payload) {
        this.loading = true;
        this.error = null;
        try {
          const result = await StudentAcademicEnrollmentService.enroll(payload);
          if (result.success) await this.fetchEnrollments({ Page: 1 });
          else this.error = result.errors;
          return result;
        } finally {
          this.loading = false;
        }
      },
      async fetchForUpsert(id) {
        this.loading = true;
        this.error = null;
        try {
          const result = await StudentAcademicEnrollmentService.getForUpsert(
            id
          );
          this.selectedEnrollment = result.success ? result.data : null;
          if (!result.success) this.error = result.errors;
          return result;
        } finally {
          this.loading = false;
        }
      },
      buildLookupFilter(type, value) {
        return StudentAcademicEnrollmentService.buildLookupFilter(type, value);
      },
      clearSelectedEnrollment() {
        this.selectedEnrollment = null;
      },
    },
  }
);
