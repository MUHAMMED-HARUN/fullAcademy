import { defineStore } from "pinia";
import teacherService from "@/Service/Person/TeacherService";

export const useTeacherStore = defineStore("teacher", {
  state: () => ({
    teachers: [],
    selectedTeacher: null,
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
    allTeachers: (state) => state.teachers,
    hasTeachers: (state) => state.teachers.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchTeachers(filter = {}) {
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

        const result = await teacherService.getAll({
          ...this.listFilter,
          Page: this.page,
          PageSize: this.pageSize,
        });

        if (result.success) {
          this.teachers = result.data?.items || [];
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
      return this.fetchTeachers({ Page: newPage });
    },

    async setPageSize(newSize) {
      this.page = 1;
      this.pageSize = newSize;
      return this.fetchTeachers({ Page: 1, PageSize: newSize });
    },

    async fetchTeacherBy(command) {
      this.loading = true;
      this.error = null;
      this.selectedTeacher = null;
      try {
        const result = await teacherService.getBy(command);
        if (result.success) {
          this.selectedTeacher = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addTeacher(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await teacherService.create(payload);
        if (result.success) {
          await this.fetchTeachers();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async updateTeacher(payload) {
      this.saving = true;
      this.error = null;
      try {
        const result = await teacherService.update(payload);
        if (result.success) {
          await this.fetchTeachers();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    async deleteTeacher(teacherID) {
      this.saving = true;
      this.error = null;
      try {
        const result = await teacherService.delete(teacherID);
        if (result.success) {
          await this.fetchTeachers();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.saving = false;
      }
    },

    clearSelectedTeacher() {
      this.selectedTeacher = null;
    },
  },
});
