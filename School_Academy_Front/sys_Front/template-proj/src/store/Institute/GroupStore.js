import { defineStore } from "pinia";
import Group_service from "@/Service/Institute/GroupService";

export const useGroupStore = defineStore("groups", {
  state: () => ({
    groups: [],
    selectedGroup: null,
    loading: false,
    error: null,

    // Paging State
    page: 1,
    pageSize: 10,
    totalCount: 0,
  }),

  getters: {
    allGroups: (state) => state.groups,
    hasGroups: (state) => state.groups.length > 0,
    isLoading: (state) => state.loading,

    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchGroups(filter = {}) {
      this.loading = true;
      this.error = null;

      if (filter.Page !== undefined) this.page = filter.Page;
      if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;

      const queryParams = {
        Page: this.page,
        PageSize: this.pageSize,
        ...filter,
      };
      const result = await Group_service.getAll(queryParams);
      if (result.success) {
        this.groups = result.data?.items || [];
        this.totalCount = result.data?.count || 0;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async setPage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.page = newPage;
        return await this.fetchGroups();
      }
    },

    async setPageSize(newSize) {
      this.pageSize = newSize;
      this.page = 1;
      return await this.fetchGroups();
    },

    async fetchGroupBy(command) {
      this.loading = true;
      this.error = null;

      const result = await Group_service.getBy(command);

      if (result.success) {
        this.selectedGroup = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },
    async GetGroupsByTeacherID(teacherID) {
      this.loading = true;
      this.error = null;
      const result = await Group_service.GetGroupsByTeacher(teacherID);

      if (result.success) {
        this.groups = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },
    async addGroup(payload) {
      this.loading = true;
      this.error = null;

      const result = await Group_service.create(payload);
      if (result.success) {
        await this.fetchGroups();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateGroup(payload) {
      this.loading = true;
      this.error = null;

      const result = await Group_service.update(payload);
      if (result.success) {
        await this.fetchGroups();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteGroup(payload) {
      this.loading = true;
      this.error = null;

      const result = await Group_service.delete(payload);
      if (result.success) {
        await this.fetchGroups();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedGroup() {
      this.selectedGroup = null;
    },
  },
});
