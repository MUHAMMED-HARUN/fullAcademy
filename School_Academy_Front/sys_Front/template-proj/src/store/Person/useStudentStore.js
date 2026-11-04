import { defineStore } from "pinia";
import studentService from "@/Service/Person/StudentService";

export const useStudentStore = defineStore("student", {
  state: () => ({
    students: [],
    selectedStudent: null,
    loading: false,
    saving: false,
    error: null,
    listError: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    listFilter: { Filter: "", OrderBy: "" },
  }),

  getters: {
    allStudents: (state) => state.students,
    hasStudents: (state) => state.students.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchStudents(filter = {}) {
      this.loading = true;
      this.error = null;
      this.listError = null;
      try {
        if (Object.hasOwn(filter, "Filter")) {
          this.listFilter.Filter = filter.Filter || "";
          this.page = 1;
        }
        if (Object.hasOwn(filter, "OrderBy")) {
          this.listFilter.OrderBy = filter.OrderBy || "";
          this.page = 1;
        }
        if (filter.Page !== undefined) this.page = filter.Page;
        if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;

        const result = await studentService.getAll({
          ...this.listFilter,
          Page: this.page,
          PageSize: this.pageSize,
        });

        if (result.success) {
          this.students = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.error = result.errors;
          this.listError = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async setPage(newPage) {
      if (newPage < 1 || newPage > this.totalPages) return;
      return this.fetchStudents({ Page: newPage });
    },

    async setPageSize(newSize) {
      this.page = 1;
      this.pageSize = newSize;
      return this.fetchStudents({ Page: 1, PageSize: newSize });
    },

    async fetchStudentBy(command) {
      this.loading = true;
      this.error = null;
      this.selectedStudent = null;
      try {
        const result = await studentService.getBy(command);
        if (result.success) {
          this.selectedStudent = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addStudent(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await studentService.create(payload);
        if (result.success) {
          await this.fetchStudents();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async updateStudent(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await studentService.update(payload);
        if (result.success) {
          await this.fetchStudents();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async deleteStudent(studentID) {
      this.saving = true;
      this.error = null;
      try {
        const result = await studentService.delete(studentID);
        if (result.success) {
          await this.fetchStudents();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    clearSelectedStudent() {
      this.selectedStudent = null;
    },
  },
});
