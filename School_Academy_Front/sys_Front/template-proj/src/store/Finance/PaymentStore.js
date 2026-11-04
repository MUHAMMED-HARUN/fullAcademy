import { defineStore } from "pinia";
import PaymentService from "@/Service/Finance/PaymentService";

export const usePaymentStore = defineStore("payments", {
  state: () => ({
    payments: [],
    selectedPayment: null,
    loading: false,
    error: null,
  }),

  getters: {
    allPayments: (state) => state.payments,
    hasPayments: (state) => state.payments.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchPayments() {
      this.loading = true;
      this.error = null;
      const result = await PaymentService.getAll();
      if (result.success) {
        this.payments = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchPaymentBy(command) {
      this.loading = true;
      this.error = null;
      const result = await PaymentService.getBy(command);
      if (result.success) {
        this.selectedPayment = result.data;
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async addPayment(payload) {
      this.loading = true;
      this.error = null;
      const result = await PaymentService.create(payload);
      if (result.success) {
        await this.fetchPayments();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async updatePayment(payload) {
      this.loading = true;
      this.error = null;
      const result = await PaymentService.update(payload);
      if (result.success) {
        await this.fetchPayments();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async DeletePayment(payload) {
      this.loading = true;
      this.error = null;
      const result = await PaymentService.delete(payload);
      if (result.success) {
        await this.fetchPayments();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    clearSelectedPayment() {
      this.selectedPayment = null;
    },
  },
});
