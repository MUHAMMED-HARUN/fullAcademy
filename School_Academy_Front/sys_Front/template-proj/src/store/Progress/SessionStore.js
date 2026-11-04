import { defineStore } from "pinia";
import Session_service from "@/Service/Progress/SessionService";

export const useSessionStore = defineStore("sessions", {
  state: () => ({
    sessions: [],
    selectedSession: null,
    loading: false,
    error: null,
    page: 1,
    pageSize: 10,
    totalCount: 0,
    currentFilter: { Filter: "", OrderBy: "" },
    lookupResults: [],
    lookupLoading: false,
    lookupError: null,
    lookupRequestVersion: 0,
  }),

  getters: {
    allsessions: (state) => state.sessions,
    hassessions: (state) => state.sessions.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },

  actions: {
    async fetchSessions(params = {}) {
      this.loading = true;
      this.error = null;
      try {
        if (params.Page !== undefined)
          this.page = Math.max(1, Number(params.Page) || 1);
        if (params.PageSize !== undefined)
          this.pageSize = Math.max(
            1,
            Math.min(100, Number(params.PageSize) || 10)
          );
        if (params.Filter !== undefined || params.OrderBy !== undefined) {
          this.currentFilter = {
            Filter: Object.prototype.hasOwnProperty.call(params, "Filter")
              ? params.Filter ?? ""
              : this.currentFilter.Filter,
            OrderBy: Object.prototype.hasOwnProperty.call(params, "OrderBy")
              ? params.OrderBy ?? ""
              : this.currentFilter.OrderBy,
          };
        }

        const result = await Session_service.getAll({
          ...this.currentFilter,
          ...params,
          Page: this.page,
          PageSize: this.pageSize,
        });
        if (result.success) {
          this.sessions = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.sessions = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async searchParents(searchTerm) {
      const requestVersion = ++this.lookupRequestVersion;
      this.lookupLoading = true;
      this.lookupError = null;
      this.lookupResults = [];
      try {
        const result = await Session_service.searchParents(searchTerm);
        if (requestVersion !== this.lookupRequestVersion) return result;

        if (result.success) {
          this.lookupResults = result.data || [];
        } else {
          this.lookupError = result.errors;
          this.lookupResults = [];
        }
        return result;
      } finally {
        if (requestVersion === this.lookupRequestVersion)
          this.lookupLoading = false;
      }
    },

    clearParentLookup() {
      this.lookupRequestVersion += 1;
      this.lookupResults = [];
      this.lookupError = null;
      this.lookupLoading = false;
    },

    async setPage(page) {
      return this.fetchSessions({ Page: page });
    },

    async setPageSize(pageSize) {
      return this.fetchSessions({ Page: 1, PageSize: pageSize });
    },

    async fetchSessiontBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Session_service.getBy(command);
        if (result.success) {
          this.selectedSession = result.data;
        } else {
          this.selectedSession = null;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addSession(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Session_service.create(payload);
        if (result.success) await this.fetchSessions();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateSession(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Session_service.update(payload);
        if (result.success) await this.fetchSessions();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeleteSession(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await Session_service.delete(payload);
        if (result.success) await this.fetchSessions();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    clearSelectedSession() {
      this.selectedSession = null;
    },
  },
});
