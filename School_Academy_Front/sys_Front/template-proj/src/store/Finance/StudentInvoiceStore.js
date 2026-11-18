import { defineStore } from "pinia";
import StudentInvoiceService from "@/Service/Finance/StudentInvoiceService";

export const useStudentInvoiceStore = defineStore("studentInvoices", {
  state: () => ({
    invoices: [],
    payableInvoices: [],
    payableInvoicesLoading: false,
    payableInvoicesError: null,
    payableInvoices: [],
    payableInvoiceCount: 0,
    payableInvoicesLoading: false,
    payableInvoicesError: null,
    totalCount: 0,
    page: 1,
    pageSize: 10,
    currentFilter: "",
    currentOrderBy: "",
    selectedInvoice: null,
    financeSummary: null,
    loading: false,
    error: null,
  }),

  getters: {
    allInvoices: (state) => state.invoices,
    hasInvoices: (state) => state.invoices.length > 0,
    isLoading: (state) => state.loading,
    totalPages: (state) =>
      Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },

  actions: {
    async fetchInvoices(params = {}) {
      this.loading = true;
      this.error = null;
      if (params.Page !== undefined)
        this.page = Math.max(1, Number(params.Page) || 1);
      if (params.PageSize !== undefined)
        this.pageSize = Math.max(
          1,
          Math.min(100, Number(params.PageSize) || 10)
        );
      if (params.Filter !== undefined) this.currentFilter = params.Filter || "";
      if (params.OrderBy !== undefined)
        this.currentOrderBy = params.OrderBy || "";
      try {
        const result = await StudentInvoiceService.getAllFilter({
          Page: this.page,
          PageSize: this.pageSize,
          Filter: this.currentFilter,
          OrderBy: this.currentOrderBy,
        });
        if (result.success) {
          this.invoices = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.invoices = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchInvoiceBy(command) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentInvoiceService.getBy(command);
        if (result.success) this.invoices = result.data || [];
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchPayableInvoices(params = {}) {
      this.payableInvoicesLoading = true;
      this.payableInvoicesError = null;
      try {
        const result = await StudentInvoiceService.getPayableInvoices({
          Page: params.Page ?? 1,
          PageSize: Math.min(100, params.PageSize ?? 10),
          Filter: params.Filter ?? "",
          OrderBy: params.OrderBy ?? "",
        });
        if (result.success) this.payableInvoices = result.data?.items || [];
        else {
          this.payableInvoices = [];
          this.payableInvoicesError = result.errors;
        }
        return result;
      } finally {
        this.payableInvoicesLoading = false;
      }
    },

    async fetchPayableInvoices(params = {}) {
      this.payableInvoicesLoading = true;
      this.payableInvoicesError = null;
      try {
        const result = await StudentInvoiceService.getPayableInvoices({
          Page: params.Page ?? 1,
          PageSize: Math.min(100, params.PageSize ?? 10),
          Filter: params.Filter ?? "",
          OrderBy: params.OrderBy ?? "",
        });
        if (result.success) {
          this.payableInvoices = result.data?.items || [];
          this.payableInvoiceCount = result.data?.count || 0;
        } else {
          this.payableInvoices = [];
          this.payableInvoiceCount = 0;
          this.payableInvoicesError = result.errors;
        }
        return result;
      } finally {
        this.payableInvoicesLoading = false;
      }
    },

    async fetchInvoiceForUpsert(id) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentInvoiceService.getForUpsert(id);
        this.selectedInvoice = result.success ? result.data : null;
        if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchByAcademicEnrollment(id) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentInvoiceService.getByAcademicEnrollment(id);
        if (result.success) this.invoices = result.data || [];
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchFinanceSummary(studentID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentInvoiceService.getFinanceSummary(studentID);
        this.financeSummary = result.success ? result.data : null;
        if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addInvoice(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentInvoiceService.create(payload);
        if (result.success) await this.fetchInvoices();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateInvoice(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentInvoiceService.update(payload);
        if (result.success) await this.fetchInvoices();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async DeleteInvoice(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await StudentInvoiceService.delete(payload);
        if (result.success) await this.fetchInvoices();
        else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    clearSelectedInvoice() {
      this.selectedInvoice = null;
    },
  },
});
