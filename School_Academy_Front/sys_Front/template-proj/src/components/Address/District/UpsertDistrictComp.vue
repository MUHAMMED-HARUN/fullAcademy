<template>
  <div>
    <div>{{ this.FormTitle }}</div>
    <form @submit.prevent.stop="onSubmit">
      <div class="form-group">
        <label>اسم الحارة:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-group">
        <label>الدولة</label>
        <select v-model="selectedCountry" @change="onCountrySelectChange">
          <option value="" disabled>اختر الدولة</option>
          <option
            v-for="country in this.countries"
            :key="country.ID"
            :value="country.ID"
          >
            {{ country.Name }}
          </option>
        </select>
      </div>
      <div class="form-group">
        <label>المدينة</label>
        <select v-model="formData.cityID">
          <option value="" disabled>اختر مدينة</option>
          <option v-for="city in this.CityList" :key="city.id" :value="city.id">
            {{ city.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.cityID" />
      </div>
      <div class="form-group">
        <button>حفظ</button>
      </div>
    </form>
  </div>
</template>
<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
export default {
  name: "UpsertDistrictComp",
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
  },

  data() {
    return {
      DistrictStore: useDistrictStore(),
      formData: {
        id: "",
        name: "",
        cityID: "",
      },
      CityList: null,
      CountryList: this.countries,
      selectedCountry: "",
    };
  },
  emits: ["onSubmit"],
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    fillForm(data) {
      this.formData = {
        id: data.id || "",
        name: data.name || "",
        cityID: data.cityID || "",
      };
    },
    onCountrySelectChange() {
      this.CityList = this.cities.filter(
        (city) => city.countryID == this.selectedCountry
      );
    },
  },
  components: {
    ValidationError,
  },
  computed: {
    FormTitle() {
      return this.formData.id ? "تعديل مدينة" : "إضافة مدينة";
    },
  },
  watch: {
    "DistrictStore.selecteddistricts": {
      handler(data) {
        if (data) this.fillForm(data);
      },
      immediate: true,
    },
  },
};
</script>
