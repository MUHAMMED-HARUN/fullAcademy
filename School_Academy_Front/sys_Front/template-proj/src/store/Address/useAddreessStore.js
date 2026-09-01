import { defineStore } from "pinia";
import address_service from "@/Service/Address/address_service";
import AddressMapper from "@/Mapper/Address/addressMapper";

export const useAddressStore = defineStore("address", {
  state: () => ({
    addresss: [],
    selectedaddress: null,
    loading: false,
    error: null,
  }),

  getters: {
    allAddress: (state) => state.addresss,
    hasAddress: (state) => state.addresss.length > 0,
    isLoading: (state) => state.loading,
  },

  actions: {
    async fetchAddresss() {
      this.loading = true;
      this.error = null;

      const result = await address_service.getAll();

      if (result.success) {
        this.addresss = result.data || [];
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    async fetchAddressBy(param) {
      this.loading = true;
      this.error = null;

      const result = await address_service.getBy(param);

      if (result.success) {
        this.selectedaddress = AddressMapper.MapFromApiDTO(result.data);
      } else {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },

    async addAddress(payload) {
      this.loading = true;
      this.error = null;

      const result = await address_service.create(payload);

      if (result.success) {
        // تحديث القائمة المحلية فوراً بعد الإضافة الناجحة
        await this.fetchAddresss();
      } else {
        this.error = result.Error;
      }

      this.loading = false;
      return result;
    },

    async updateAddress(payload) {
      this.loading = true;
      this.error = null;
      const result = await address_service.update(payload);
      if (result.success) {
        await this.fetchAddresss();
      } else {
        this.error = result.errors;
      }
      this.loading = false;
      return result;
    },

    clearSelectedAddress() {
      this.selectedaddress = null;
    },
  },
});
