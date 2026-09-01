<template>
  <div>
    <div>{{ this.FormTitle }}</div>
    <form @submit.prevent.stop="onSubmit">
      <div class="form-group">
        <label>الدولة</label>
        <select
          v-model="formData.countryID"
          @change="onCountryChange(formData.countryID)"
        >
          <option value="" disabled>اختر دولة</option>
          <option
            v-for="country in this.countries"
            :key="country.ID"
            :value="country.ID"
          >
            {{ country.Name }}
          </option>
        </select>
        <ValidationError :messages="errors?.countryID" />
      </div>

      <div class="form-group">
        <label>المدينة</label>
        <select
          v-model="formData.cityID"
          @change="onCityChange(formData.cityID)"
        >
          <option value="" disabled>اختر مدينة</option>
          <option
            v-for="citiy in this.cities"
            :key="citiy.id"
            :value="citiy.id"
          >
            {{ citiy.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.cityID" />
      </div>

      <div class="form-group">
        <label>الجارة</label>
        <select
          v-model="formData.districtID"
          @change="onDistrictChange(formData.districtID)"
        >
          <option value="" disabled>اختر حارة</option>
          <option
            v-for="distric in this.districts"
            :key="distric.id"
            :value="distric.id"
          >
            {{ distric.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.districtID" />
      </div>

      <div class="form-group">
        <label>الشارع</label>
        <select
          v-model="formData.neighborhoodID"
          @change="onNeighboodChange(formData.neighborhoodID)"
        >
          <option value="" disabled>اختر شارع</option>
          <option
            v-for="neighborhood in this.neighborhoods"
            :key="neighborhood.id"
            :value="neighborhood.id"
          >
            {{ neighborhood.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.neighborhoodID" />
      </div>
      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>
      <div class="form-group">
        <button>حفظ</button>
      </div>
    </form>
  </div>
</template>
<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useAddressStore } from "@/store/Address/useAddreessStore";
export default {
  name: "UpsertAddressComp",
  props: {
    errors: {
      type: Object,
      required: false,
    },
    countries: {
      type: Array,
      required: true,
    },
    cities: {
      type: Array,
      required: true,
    },
    districts: {
      type: Array,
      required: true,
    },
    neighborhoods: {
      type: Array,
      required: true,
    },
  },

  data() {
    return {
      AddressStore: useAddressStore(),
      formData: {
        id: "",
        countryID: "",
        cityID: "",
        districtID: "",
        neighborhoodID: "",
        notes: "",
      },
    };
  },
  emits: [
    "onSubmit",
    "CityChange",
    "CountryChange",
    "DistrictChange",
    "NeighboodChange",
  ],
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onCountryChange(e) {
      alert(e);
      this.$emit("CountryChange", e);
    },
    onCityChange(e) {
      this.$emit("CityChange", e);
    },
    onDistrictChange(e) {
      this.$emit("DistrictChange", e);
    },
    onNeighboodChange(e) {
      this.$emit("NeighboodChange", e);
    },
  },
  components: {
    ValidationError,
  },
  computed: {
    FormTitle() {
      return this.formData.id ? "تعديل عنوان" : "إضافة عنوان";
    },
  },
};
</script>
