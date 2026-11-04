import { defineStore } from "pinia";
import StudentInvoiceService from "@/Service/Finance/StudentInvoiceService";

export const useStudentInvoiceStore = defineStore("studentInvoices", {
  state: () => ({
    invoices: [],
    selectedInvoice: null,
    loading: false,
    error: null,
  }),

  getters: {
    allInvoices: (state) => state.invoices,
    hasInvoices: (state) => state.invoices.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchInvoices() {
      this.loading = true;
      this.error = null;
      const result = await StudentInvoiceService.getAll();
      if (result.success) {
        this.invoices = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchInvoiceBy(command) {
      this.loading = true;
      this.error = null;
      const result = await StudentInvoiceService.getBy(command);
      if (result.success) {
        this.selectedInvoice = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addInvoice(payload) {
      this.loading = true;
      this.error = null;
      const result = await StudentInvoiceService.create(payload);
      if (result.success) {
        await this.fetchInvoices();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async updateInvoice(payload) {
      this.loading = true;
      this.error = null;
      const result = await StudentInvoiceService.update(payload);
      if (result.success) {
        await this.fetchInvoices();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeleteInvoice(payload) {
      this.loading = true;
      this.error = null;
      const result = await StudentInvoiceService.delete(payload);
      if (result.success) {
        await this.fetchInvoices();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    clearSelectedInvoice() {
      this.selectedInvoice = null;
    },
  },
});
