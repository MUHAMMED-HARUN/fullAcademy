import { defineStore } from "pinia";
import student_service from "@/Service/Person/StudentService";

export const useStudentStore = defineStore("student", {
  state: () => ({
    students: [],
    selectedStudent: null,
    loading: false,
    error: null,

    // 🔹 حالات التصفح (Paging State)
    page: 1,
    pageSize: 10,
    totalCount: 0,
  }),

  getters: {
    allstudents: (state) => state.students,
    hasStudents: (state) => state.students.length > 0,
    isLoading: (state) => state.loading,

    // 🔹 حساب إجمالي عدد الصفحات
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchStudents(filter = {}) {
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

      const result = await student_service.getAll(queryParams);
      if (result.success) {
        this.students = result.data?.items || [];
        this.totalCount = result.data?.count || 0;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    // 🔹 التحكم برقم الصفحة
    async setPage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.page = newPage;
        return await this.fetchStudents();
      }
    },

    // 🔹 التحكم بحجم الصفحة
    async setPageSize(newSize) {
      this.pageSize = newSize;
      this.page = 1;
      return await this.fetchStudents();
    },

    async fetchStudentBy(command) {
      this.loading = true;
      this.error = null;

      const result = await student_service.getBy(command);

      if (result.success) {
        this.selectedStudent = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addStudent(payload) {
      this.loading = true;
      this.error = null;

      const result = await student_service.create(payload);
      if (result.success) {
        await this.fetchStudents();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateStudent(payload) {
      this.loading = true;
      this.error = null;
      const result = await student_service.update(payload);
      if (result.success) {
        await this.fetchStudents();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteStudent(payload) {
      this.loading = true;
      this.error = null;

      const result = await student_service.delete(payload);
      if (result.success) {
        await this.fetchStudents();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    clearSelectedStudent() {
      this.selectedStudent = null;
    },
  },
});
