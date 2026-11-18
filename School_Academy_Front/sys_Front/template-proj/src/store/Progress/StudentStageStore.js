import { defineStore } from "pinia";
import StudentStageService from "@/Service/Progress/StudentStageService";

export const useStudentStageStore = defineStore("StudentStages", {
  state: () => ({
    StudentStages: [],
    selectedStudentStage: null,
    loading: false,
    error: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    studentAcademicEnrollmentID: "",
    filter: "",
  }),
  getters: {
    allStudentStages: (state) => state.StudentStages,
    hasStudentStages: (state) => state.StudentStages.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },
  actions: {
    async fetchStudentStages(params = {}) {
      this.loading = true;
      this.error = null;
      if (params.Page !== undefined)
        this.page = Math.max(1, Number(params.Page) || 1);
      if (params.PageSize !== undefined)
        this.pageSize = Math.max(
          1,
          Math.min(100, Number(params.PageSize) || 10)
        );
      if (Object.hasOwn(params, "StudentAcademicEnrollmentID")) {
        this.studentAcademicEnrollmentID =
          params.StudentAcademicEnrollmentID || "";
        this.page = 1;
      }
      if (Object.hasOwn(params, "Filter")) this.filter = params.Filter || "";

      try {
        const result = await StudentStageService.getAll({
          Page: this.page,
          PageSize: this.pageSize,
          StudentAcademicEnrollmentID:
            this.studentAcademicEnrollmentID || undefined,
          Filter: this.filter,
        });
        if (result.success) {
          this.StudentStages = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.StudentStages = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },
    async setPage(page) {
      return this.fetchStudentStages({ Page: page });
    },
    async setPageSize(pageSize) {
      return this.fetchStudentStages({ Page: 1, PageSize: pageSize });
    },
    async fetchStudentStagetBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentStageService.getBy(command);
        if (result.success) this.selectedStudentStage = result.data;
        else {
          this.selectedStudentStage = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },
    async addStudentStage(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentStageService.create(payload);
        if (result.success) await this.fetchStudentStages();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    async updateStudentStage(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentStageService.update(payload);
        if (result.success) await this.fetchStudentStages();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    async DeleteStudentStage(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentStageService.delete(payload);
        if (result.success) await this.fetchStudentStages();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    clearSelectedStudentStage() {
      this.selectedStudentStage = null;
    },
  },
});
