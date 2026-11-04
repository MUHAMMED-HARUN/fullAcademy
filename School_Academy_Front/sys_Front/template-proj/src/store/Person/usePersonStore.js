import { defineStore } from "pinia";
import person_service from "@/Service/Person/PersonService";
import PersonMapper from "@/Mapper/Person/PersonMapper";
import TResult from "@/Generic/TResult";

export const usePersonStore = defineStore("person", {
  state: () => ({
    persons: [],
    selectedPerson: null,
    loading: false,
    error: null,
    listError: null,
    personSearchResults: [],
    personSearchLoading: false,
    personSearchError: null,
    personSearchRequestId: 0,

    // 🔹 حالات التصفح (Paging State)
    page: 1,
    pageSize: 10,
    totalCount: 0,
  }),

  getters: {
    allPersons: (state) => state.persons,
    hasPersons: (state) => state.persons.length > 0,
    isLoading: (state) => state.loading,

    // 🔹 حساب إجمالي عدد الصفحات
    totalPages: (state) => Math.ceil(state.totalCount / state.pageSize) || 1,
  },

  actions: {
    async fetchPersons(filter = {}) {
      this.loading = true;
      this.error = null;
      this.listError = null;
      try {
        if (filter.Page !== undefined) this.page = filter.Page;
        if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;

        const queryParams = {
          Page: this.page,
          PageSize: this.pageSize,
          ...filter,
        };

        const result = await person_service.getAll(queryParams);
        if (result.success) {
          this.persons = result.data.items || [];
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

    // 🔹 التحكم برقم الصفحة
    async setPage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        this.page = newPage;
        return await this.fetchPersons();
      }
    },

    // 🔹 التحكم بحجم الصفحة
    async setPageSize(newSize) {
      this.pageSize = newSize;
      this.page = 1;
      return await this.fetchPersons();
    },

    async fetchPersonBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await person_service.getBy(command);

        if (result.success) {
          this.selectedPerson = PersonMapper.MapFromApiDTO(result.data);
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchPersonForUpsert(personID) {
      this.loading = true;
      this.error = null;

      try {
        const result = await person_service.getForUpsert(personID);
        if (result.success) {
          this.selectedPerson = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async searchPersonCandidates(criteria) {
      const requestId = ++this.personSearchRequestId;
      this.personSearchLoading = true;
      this.personSearchError = null;
      this.personSearchResults = [];
      this.selectedPerson = null;

      try {
        const result = await person_service.searchForCandidates(criteria);
        if (requestId !== this.personSearchRequestId) return result;

        if (result.success) {
          this.personSearchResults = result.data?.items || [];
        } else {
          this.personSearchError = result.errors;
        }
        return result;
      } catch (error) {
        const result = new TResult(null, false, {
          errors: error?.message || "تعذر البحث عن الشخص",
        });
        if (requestId === this.personSearchRequestId) {
          this.personSearchError = result.errors;
        }
        return result;
      } finally {
        if (requestId === this.personSearchRequestId) {
          this.personSearchLoading = false;
        }
      }
    },

    selectPersonCandidate(person) {
      this.selectedPerson = person || null;
    },

    clearPersonSelection() {
      this.resetPersonCandidateSearch();
    },

    clearSelectedPersonCandidate() {
      this.selectedPerson = null;
    },

    resetPersonCandidateSearch() {
      this.personSearchRequestId += 1;
      this.selectedPerson = null;
      this.personSearchResults = [];
      this.personSearchError = null;
      this.personSearchLoading = false;
    },

    async addPerson(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await person_service.create(payload);
        if (result.success) {
          await this.fetchPersons();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updatePerson(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await person_service.update(payload);
        if (result.success) {
          await this.fetchPersons();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeletePerson(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await person_service.delete(payload);
        if (result.success) {
          await this.fetchPersons();
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    clearSelectedPerson() {
      this.selectedPerson = null;
    },
  },
});
