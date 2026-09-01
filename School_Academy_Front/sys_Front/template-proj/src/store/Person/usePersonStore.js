import { defineStore } from "pinia";
import person_service from "@/Service/Person/PersonService";
import PersonMapper from "@/Mapper/Person/PersonMapper";

export const usePersonStore = defineStore("person", {
  state: () => ({
    persons: [],
    selectedPerson: null,
    loading: false,
    error: null,

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

      // تحديث رقم الصفحة والحجم عند التمرير
      if (filter.Page !== undefined) this.page = filter.Page;
      if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;

      const queryParams = {
        Page: this.page,
        PageSize: this.pageSize,
        ...filter,
      };

      const result = await person_service.getAll(queryParams);
      console.log("result store", result);
      if (result.success) {
        this.persons = result.data.items || [];
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

      const result = await person_service.getBy(command);

      if (result.success) {
        var data = PersonMapper.MapFromApiDTO(result.data);
        this.selectedPerson = data;
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addPerson(payload) {
      this.loading = true;
      this.error = null;

      const result = await person_service.create(payload);
      if (result.success) {
        await this.fetchPersons();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async updatePerson(payload) {
      this.loading = true;
      this.error = null;

      const result = await person_service.update(payload);
      if (result.success) {
        await this.fetchPersons();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async DeletePerson(payload) {
      this.loading = true;
      this.error = null;

      const result = await person_service.delete(payload);
      if (result.success) {
        await this.fetchPersons();
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    clearSelectedPerson() {
      this.selectedPerson = null;
    },
  },
});
