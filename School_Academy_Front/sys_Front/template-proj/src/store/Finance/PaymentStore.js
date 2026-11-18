import { defineStore } from "pinia";
import PaymentService from "@/Service/Finance/PaymentService";

export const usePaymentStore = defineStore("payments", {
  state: () => ({
    payments: [],
    totalCount: 0,
    page: 1,
    pageSize: 10,
    currentFilter: "",
    currentOrderBy: "",
    paymentsByInvoice: [],
    selectedPayment: null,
    loading: false,
    error: null,
  }),
  getters: {
    allPayments: (state) => state.payments,
    isLoading: (state) => state.loading,
    totalPages: (state) => Math.max(1, Math.ceil(state.totalCount / state.pageSize)),
  },
  actions: {
    async fetchPayments(params = {}) {
      this.loading = true;
      this.error = null;
      if (params.Page !== undefined) this.page = Math.max(1, Number(params.Page) || 1);
      if (params.PageSize !== undefined) this.pageSize = Math.max(1, Math.min(100, Number(params.PageSize) || 10));
      if (params.Filter !== undefined) this.currentFilter = params.Filter || "";
      if (params.OrderBy !== undefined) this.currentOrderBy = params.OrderBy || "";
      try {
        const result = await PaymentService.getAllFilter({
          Page: this.page,
          PageSize: this.pageSize,
          Filter: this.currentFilter,
          OrderBy: this.currentOrderBy,
        });
        if (result.success) {
          this.payments = result.data?.items || [];
          this.totalCount = result.data?.count || 0;
        } else {
          this.payments = [];
          this.totalCount = 0;
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchPaymentsByInvoice(invoiceID) {
      this.loading = true;
      this.error = null;
      try {
        const result = await PaymentService.getPaymentsByInvoice(invoiceID);
        if (result.success) this.paymentsByInvoice = result.data || [];
        else {
          this.paymentsByInvoice = [];
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async fetchPaymentBy(query) {
      this.loading = true;
      this.error = null;
      try {
        const result = await PaymentService.getBy(query);
        this.selectedPayment = result.success ? result.data : null;
        if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addPayment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await PaymentService.create(payload);
        if (result.success) {
          await this.fetchPayments();
          await this.fetchPaymentsByInvoice(payload.studentInvoiceID);
        } else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    async reversePayment(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await PaymentService.reverse(payload);
        if (result.success) {
          await this.fetchPayments();
          if (this.selectedPayment?.studentInvoiceID)
            await this.fetchPaymentsByInvoice(this.selectedPayment.studentInvoiceID);
        } else this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },

    clearSelectedPayment() {
      this.selectedPayment = null;
    },
  },
});
