<template>
  <div>
    <div>{{ this.FormTitle }}</div>
    <form @submit.prevent.stop="onSubmit">
      <div class="form-group">
        <label>اسم الدولة:</label>
        <input v-model="formData.Name" type="text" />
        <ValidationError :messages="errors?.Name" />
      </div>
      <div class="form-group">
        <label>رمز الدولة:</label>
        <input v-model="formData.Code" type="text" />
        <ValidationError :messages="errors?.Code" />
      </div>
      <div class="form-group">
        <button>حفظ</button>
      </div>
    </form>
  </div>
</template>
<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useCountryStore } from "@/store/Address/useCountryStore";
export default {
  name: "UpsertCountryComp",
  props: {
    errors: {
      type: Object,
      required: false,
    },
  },
  data() {
    return {
      CountryStore: useCountryStore(),
      formData: {
        Name: "",
        Code: "",
        ID: "",
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
        ID: data.ID || "",
        Name: data.Name || "",
        Code: data.Code || "",
      };
    },
  },
  components: {
    ValidationError,
  },
  computed: {
    FormTitle() {
      return this.$route.params.ID ? "تعديل الدولة" : "إضافة دولة";
    },
  },
  watch: {
    "CountryStore.selectedCountry": {
      handler(Country) {
        if (Country) this.fillForm(Country);
      },
      immediate: true,
    },
  },
};
</script>
