<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المعهد:</label>
        <input v-model="formData.name" type="text" :disabled="loading" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.note" type="text" :disabled="loading" />
        <ValidationError :messages="errors?.note" />
      </div>

      <!-- تضمين مكون العنوان الفرعي بربط v-model -->
      <fieldset class="address-section">
        <legend>بيانات العنوان</legend>
        <UpsertAddressComp
          v-model="formData.address"
          :errors="errors?.address"
          :countries="countries"
          :countries-loading="countriesLoading"
          :countries-error="countriesError"
          :city-results="cityResults"
          :city-search-loading="citySearchLoading"
          :city-search-error="citySearchError"
          :district-results="districtResults"
          :district-search-loading="districtSearchLoading"
          :district-search-error="districtSearchError"
          :neighborhood-results="neighborhoodResults"
          :neighborhood-search-loading="neighborhoodSearchLoading"
          :neighborhood-search-error="neighborhoodSearchError"
          :loading="loading"
          @search-city="$emit('search-city', $event)"
          @clear-city-search="$emit('clear-city-search')"
          @search-district="$emit('search-district', $event)"
          @clear-district-search="$emit('clear-district-search')"
          @search-neighborhood="$emit('search-neighborhood', $event)"
          @clear-neighborhood-search="$emit('clear-neighborhood-search')"
        />
      </fieldset>

      <div class="form-group">
        <button type="submit" :disabled="loading">
          {{ loading ? "جارٍ الحفظ..." : "حفظ" }}
        </button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import UpsertAddressComp from "@/components/Address/Address/upsertAddressItem.vue";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";

export default {
  name: "UpsertInstituteComp",
  components: {
    ValidationError,
    UpsertAddressComp,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    countries: { type: Array, default: () => [] },
    countriesLoading: { type: Boolean, default: false },
    countriesError: { type: [String, Array, Object], default: null },
    cityResults: { type: Array, default: () => [] },
    citySearchLoading: { type: Boolean, default: false },
    citySearchError: { type: [String, Array, Object], default: null },
    districtResults: { type: Array, default: () => [] },
    districtSearchLoading: { type: Boolean, default: false },
    districtSearchError: { type: [String, Array, Object], default: null },
    neighborhoodResults: { type: Array, default: () => [] },
    neighborhoodSearchLoading: { type: Boolean, default: false },
    neighborhoodSearchError: { type: [String, Array, Object], default: null },
    loading: { type: Boolean, default: false },
    address: {
      type: Object,
      default: () => ({
        id: "",
        countryID: "",
        cityID: "",
        districtID: "",
        neighborhoodID: "",
        notes: "",
      }),
    },
  },
  data() {
    return {
      InstituteStore: useInstituteStore(),
      formData: {
        instituteID: "",
        name: "",
        note: "",

        address: {
          id: "",
          countryID: "",
          cityID: "",
          districtID: "",
          neighborhoodID: "",
          notes: "",
          countryName: "",
          cityName: "",
          districtName: "",
          neighborhoodName: "",
        },
      },
    };
  },
  emits: [
    "onSubmit",
    "search-city",
    "clear-city-search",
    "search-district",
    "clear-district-search",
    "search-neighborhood",
    "clear-neighborhood-search",
  ],
  computed: {
    FormTitle() {
      return this.formData.instituteID ? "تعديل معهد" : "إضافة معهد";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", {
        ...this.formData,
        address: { ...this.formData.address },
      });
    },
    fillSelectedInstitute() {
      if (this.InstituteStore.selectedInstitute && this.address) {
        const entity = this.InstituteStore.selectedInstitute;
        this.formData = {
          instituteID: entity.instituteID || "",
          name: entity.name || "",
          note: entity.note || "",
          address: {
            id: this.address.id || "",
            countryID: this.address?.countryID || "",
            cityID: this.address?.cityID || "",
            districtID: this.address?.districtID || "",
            neighborhoodID: this.address?.neighborhoodID || "",
            notes: this.address?.notes || "",
            countryName: this.address?.countryName || "",
            cityName: this.address?.cityName || "",
            districtName: this.address?.districtName || "",
            neighborhoodName: this.address?.neighborhoodName || "",
          },
        };
      }
    },
  },

  mounted() {
    this.fillSelectedInstitute();
  },
};
</script>
