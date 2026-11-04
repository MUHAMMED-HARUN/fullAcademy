<template>
  <div>
    <h2 class="mb-4 text-xl font-bold text-[var(--primary-color)]">
      {{ FormTitle }}
    </h2>
    <form class="space-y-4" @submit.prevent.stop="onSubmit">
      <div class="form-group">
        <label for="country-name">اسم الدولة:</label>
        <input
          id="country-name"
          v-model="formData.name"
          class="form-control"
          type="text"
          :disabled="loading"
        />
        <ValidationError :messages="errors?.name" />
      </div>
      <div class="form-group">
        <label for="country-code">رمز الدولة:</label>
        <input
          id="country-code"
          v-model="formData.code"
          class="form-control"
          type="text"
          :disabled="loading"
        />
        <ValidationError :messages="errors?.code" />
      </div>
      <div class="form-group">
        <button
          class="btn btn-primary"
          type="submit"
          :disabled="loading"
          :aria-busy="loading"
        >
          {{ loading ? "جارٍ الحفظ..." : "حفظ" }}
        </button>
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
      type: [Object, String, Array],
      default: () => ({}),
    },
    loading: { type: Boolean, default: false },
  },
  data() {
    return {
      CountryStore: useCountryStore(),
      formData: {
        name: "",
        code: "",
        id: "",
      },
    };
  },
  emits: ["onSubmit"],
  methods: {
    onSubmit() {
      this.$emit("onSubmit", { ...this.formData });
    },
    fillForm(data) {
      this.formData = {
        id: data.id || data.ID || "",
        name: data.name || data.Name || "",
        code: data.code || data.Code || "",
      };
    },
    resetForm() {
      this.formData = { id: "", name: "", code: "" };
    },
  },
  components: {
    ValidationError,
  },
  computed: {
    FormTitle() {
      return this.formData.id ? "تعديل الدولة" : "إضافة دولة";
    },
  },
  watch: {
    "CountryStore.selectedCountry": {
      handler(Country) {
        if (Country) this.fillForm(Country);
        else this.resetForm();
      },
      immediate: true,
    },
  },
};
</script>
