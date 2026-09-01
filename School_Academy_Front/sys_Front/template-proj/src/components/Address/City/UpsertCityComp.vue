<template>
  <div>
    <div>{{ this.FormTitle }}</div>
    <form @submit.prevent.stop="onSubmit">
      <div class="form-group">
        <label>اسم المدينة:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>
      <div class="form-group">
        <label>رمز المدينة:</label>
        <input v-model="formData.code" type="text" />
        <ValidationError :messages="errors?.code" />
      </div>
      <div class="form-group">
        <label>الدولة</label>
        <select v-model="formData.countryID">
          <option value="" disabled>اختر الدولة</option>
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
        <button>حفظ</button>
      </div>
    </form>
  </div>
</template>
<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useCityStore } from "@/store/Address/useCityStore";
export default {
  name: "UpsertCityComp",
  props: {
    errors: {
      type: Object,
      required: false,
    },
    countries: {
      type: Array,
      required: true,
    },
  },

  data() {
    return {
      CityStore: useCityStore(),
      formData: {
        id: "",
        name: "",
        code: "",
        countryID: "",
      },
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
        code: data.code || "",
        countryID: data.countryID || "",
      };
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
    "CityStore.selectedCity": {
      handler(city) {
        if (city) this.fillForm(city);
      },
      immediate: true,
    },
  },
};
</script>
