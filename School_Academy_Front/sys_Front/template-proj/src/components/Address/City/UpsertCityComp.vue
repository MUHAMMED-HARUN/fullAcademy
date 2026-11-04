<template>
  <section class="space-y-4" dir="rtl" aria-labelledby="city-form-title">
    <header>
      <h2
        id="city-form-title"
        class="text-xl font-bold text-[var(--primary-color)]"
      >
        {{ formTitle }}
      </h2>
    </header>

    <form class="space-y-4" @submit.prevent="onSubmit">
      <fieldset class="space-y-4 border-0 p-0" :disabled="loading">
        <div class="form-group">
          <label for="city-name">اسم المدينة:</label>
          <input
            id="city-name"
            v-model="formData.name"
            class="form-control"
            type="text"
          />
          <ValidationError :messages="errors?.name" />
        </div>
        <div class="form-group">
          <label for="city-code">رمز المدينة:</label>
          <input
            id="city-code"
            v-model="formData.code"
            class="form-control"
            type="text"
          />
          <ValidationError :messages="errors?.code" />
        </div>
        <div class="form-group">
          <label for="city-country">الدولة:</label>
          <select
            id="city-country"
            v-model="formData.countryID"
            class="form-select"
            :aria-invalid="!!errors?.countryID"
            :disabled="loading || countriesLoading"
          >
            <option value="" disabled>اختر دولة</option>
            <option
              v-for="country in countries"
              :key="country.ID || country.id"
              :value="country.ID || country.id"
            >
              {{ country.Name || country.name }}
            </option>
          </select>
          <p
            v-if="countriesLoading"
            class="text-sm text-[var(--text-muted)]"
            role="status"
          >
            جارٍ تحميل الدول...
          </p>
          <ValidationError :messages="errors?.countryID" />
        </div>
      </fieldset>

      <footer
        class="flex justify-end border-t border-[var(--border-color)] pt-4"
      >
        <button
          class="btn btn-primary"
          type="submit"
          :disabled="loading || countriesLoading"
        >
          {{ loading ? "جارٍ الحفظ..." : "حفظ المدينة" }}
        </button>
      </footer>
    </form>
  </section>
</template>

<script setup>
import { computed, reactive, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  errors: { type: [Object, String, Array], default: () => ({}) },
  countries: { type: Array, default: () => [] },
  countriesLoading: { type: Boolean, default: false },
  countryData: { type: Object, default: null },
  cityData: { type: Object, default: null },
  loading: { type: Boolean, default: false },
});

const emit = defineEmits(["onSubmit"]);
const formData = reactive({ id: "", name: "", code: "", countryID: "" });

const formTitle = computed(() => (formData.id ? "تعديل مدينة" : "إضافة مدينة"));

function fillForm(city) {
  formData.id = city?.id || city?.cityID || city?.ID || "";
  formData.name = city?.name || city?.cityName || city?.Name || "";
  formData.code = city?.code || city?.cityCode || city?.Code || "";
  formData.countryID = city?.countryID || city?.CountryID || "";
}

function onSubmit() {
  emit("onSubmit", { ...formData });
}

watch(
  () => props.cityData,
  (city) => fillForm(city),
  { immediate: true, deep: true }
);

watch(
  () => props.countryData,
  (country) => {
    const countryID = country?.id || country?.ID;
    if (countryID) formData.countryID = countryID;
  },
  { immediate: true, deep: true }
);
</script>
