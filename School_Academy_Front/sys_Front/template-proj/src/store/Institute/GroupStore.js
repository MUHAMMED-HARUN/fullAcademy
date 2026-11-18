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
    filter: "",
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

      if (filter.Page !== undefined)
        this.page = Math.max(1, Number(filter.Page) || 1);
      if (filter.PageSize !== undefined)
        this.pageSize = Math.max(
          1,
          Math.min(100, Number(filter.PageSize) || 10)
        );
      if (Object.hasOwn(filter, "Filter")) this.filter = filter.Filter || "";

      try {
        const result = await Group_service.getAll({
          Page: this.page,
          PageSize: this.pageSize,
          Filter: this.filter,
        });
        if (result.success) {
          this.groups = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.groups = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
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

      try {
        const result = await Group_service.getBy(command);
        if (result.success) {
          this.selectedGroup = result.data;
        } else {
          this.selectedGroup = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },
    async fetchMyTeachingGroups() {
      this.loading = true;
      this.error = null;
      try {
        const result = await Group_service.getMyTeachingGroups();
        if (result.success) {
          this.groups = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.groups = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },
    async addGroup(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await Group_service.create(payload);
        if (result.success) await this.fetchGroups();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateGroup(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await Group_service.update(payload);
        if (result.success) await this.fetchGroups();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeleteGroup(payload) {
      this.loading = true;
      this.error = null;

      try {
        const result = await Group_service.delete(payload);
        if (result.success) await this.fetchGroups();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    clearSelectedGroup() {
      this.selectedGroup = null;
    },
  },
});
