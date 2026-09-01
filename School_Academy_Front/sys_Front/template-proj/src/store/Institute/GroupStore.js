import { defineStore } from "pinia";
import Group_service from "@/Service/Institute/GroupService";

export const useGroupStore = defineStore("groups", {
  state: () => ({
    groups: [],
    selectedGroup: null,
    loading: false,
    error: null,
  }),

  getters: {
    allGroups: (state) => state.groups,
    hasGroups: (state) => state.groups.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchGroups() {
      this.loading = true;
      this.error = null;

      const result = await Group_service.getAll();
      if (result.success) {
        this.groups = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
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
