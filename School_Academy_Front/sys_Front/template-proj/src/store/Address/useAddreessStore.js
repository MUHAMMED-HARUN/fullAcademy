import { defineStore } from "pinia";
import address_service from "@/Service/Address/address_service";

export const useAddressStore = defineStore("address", {
  state: () => ({
    selectedaddress: null,
    loading: false,
    error: null,
  }),

  getters: {
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchAddressBy(param, paramType) {
      this.loading = true;
      this.error = null;
      this.selectedaddress = null;
      try {
        const result = await address_service.getBy(
          typeof param === "object"
            ? param
            : { Param: param, ParamType: paramType }
        );
        if (result.success) {
          this.selectedaddress = result.data;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async addAddress(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await address_service.create(payload);
        if (result.success) {
          this.selectedaddress = {
            ...payload,
            id: result.data || payload.id || "",
          };
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    async updateAddress(payload) {
      this.loading = true;
      this.error = null;
      try {
        const result = await address_service.update(payload);
        if (result.success) {
          this.selectedaddress = payload;
        } else {
          this.error = result.errors;
        }
        return result;
      } finally {
        this.loading = false;
      }
    },

    clearSelectedAddress() {
      this.selectedaddress = null;
    },
  },
});
