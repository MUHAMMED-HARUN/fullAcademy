import { defineStore } from "pinia";
import Teacher_service from "@/Service/Person/TeacherService";

export const useTeacherStore = defineStore("Teacher", {
  state: () => ({
    Teachers: [],
    selectedTeacher: null,
    loading: false,
    error: null,

    // 🔹 حالات التصفح (Paging State)
    page: 1,
    pageSize: 10,
    totalCount: 0,
  }),

  getters: {
    allTeachers: (state) => state.Teachers,
    hasTeachers: (state) => state.Teachers.length > 0,
    isLoading: (state) => state.loading,

    // 🔹 حساب إجمالي عدد الصفحات
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchTeachers(filter = {}) {
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

      const result = await Teacher_service.getAll(queryParams);
      if (result.success) {
        this.Teachers = result.data?.items || [];
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
        return await this.fetchTeachers();
      }
    },

    // 🔹 التحكم بحجم الصفحة
    async setPageSize(newSize) {
      this.pageSize = newSize;
      this.page = 1;
      return await this.fetchTeachers();
    },

    async fetchTeacherBy(command) {
      this.loading = true;
      this.error = null;

      const result = await Teacher_service.getBy(command);

      if (result.success) {
        this.selectedTeacher = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addTeacher(payload) {
      this.loading = true;
      this.error = null;

      const result = await Teacher_service.create(payload);
      if (result.success) {
        await this.fetchTeachers();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateTeacher(payload) {
      this.loading = true;
      this.error = null;
      const result = await Teacher_service.update(payload);
      if (result.success) {
        await this.fetchTeachers();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteTeacher(payload) {
      this.loading = true;
      this.error = null;

      const result = await Teacher_service.delete(payload);
      if (result.success) {
        await this.fetchTeachers();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    clearSelectedTeacher() {
      this.selectedTeacher = null;
    },
  },
});
