<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المعهد:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.note" type="text" />
        <ValidationError :messages="errors?.note" />
      </div>

      <!-- تضمين مكون العنوان الفرعي بربط v-model -->
      <fieldset class="address-section">
        <legend>بيانات العنوان</legend>
        <UpsertAddressComp
          :modelValue="formData.address"
          :errors="errors?.address"
          :countries="countries"
          :cities="cities"
          :districts="districts"
          :neighborhoods="neighborhoods"
          @CountryChange="$emit('CountryChange', $event)"
          @CityChange="$emit('CityChange', $event)"
          @DistrictChange="$emit('DistrictChange', $event)"
          @NeighboodChange="$emit('NeighboodChange', $event)"
          @updatemodelValue="fillAddress"
        />
      </fieldset>

      <div class="form-group">
        <button type="submit">حفظ</button>
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
    cities: { type: Array, default: () => [] },
    districts: { type: Array, default: () => [] },
    neighborhoods: { type: Array, default: () => [] },
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
        },
      },
    };
  },
  emits: [
    "onSubmit",
    "CountryChange",
    "CityChange",
    "DistrictChange",
    "NeighboodChange",
  ],
  computed: {
    FormTitle() {
      return this.formData.InstituteStore ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
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
          },
        };
      }
    },
    fillAddress(field) {
      this.formData.address = field;
    },
  },

  mounted() {
    this.fillSelectedInstitute();
  },
};
</script>
