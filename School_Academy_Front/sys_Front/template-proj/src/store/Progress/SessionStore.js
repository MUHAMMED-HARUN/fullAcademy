import { defineStore } from "pinia";
import Session_service from "@/Service/Progress/SessionService";

export const useSessionStore = defineStore("sessions", {
  state: () => ({
    sessions: [],
    selectedSession: null,
    loading: false,
    error: null,
  }),

  getters: {
    allsessions: (state) => state.enrollments,
    hassessions: (state) => state.enrollments.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchSessions() {
      this.loading = true;
      this.error = null;

      const result = await Session_service.getAll();
      if (result.success) {
        this.sessions = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchSessiontBy(command) {
      this.loading = true;
      this.error = null;

      const result = await Session_service.getBy(command);

      if (result.success) {
        this.selectedSession = result.data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addSession(payload) {
      this.loading = true;
      this.error = null;

      const result = await Session_service.create(payload);
      if (result.success) {
        await this.fetchSessions();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updateSession(payload) {
      this.loading = true;
      this.error = null;

      const result = await Session_service.update(payload);
      if (result.success) {
        await this.fetchSessions();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteSession(payload) {
      this.loading = true;
      this.error = null;

      const result = await Session_service.delete(payload);
      if (result.success) {
        await this.fetchSessions();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },
    clearSelectedSession() {
      this.selectedSession = null;
    },
  },
});
