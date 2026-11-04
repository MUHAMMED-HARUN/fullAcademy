<template>
  <section class="space-y-5" dir="rtl" aria-labelledby="district-form-title">
    <header>
      <h2
        id="district-form-title"
        class="text-xl font-bold text-[var(--primary-color)]"
      >
        {{ formTitle }}
      </h2>
      <p class="mt-1 text-sm text-[var(--text-muted)]">
        اختر الدولة ثم ابحث عن المدينة التابعة لها.
      </p>
    </header>

    <form class="space-y-4" @submit.prevent="onSubmit">
      <fieldset class="space-y-4 border-0 p-0" :disabled="loading">
        <div class="form-group">
          <label for="district-name">اسم المنطقة:</label>
          <input
            id="district-name"
            v-model="formData.name"
            class="form-control"
            type="text"
          />
          <ValidationError :messages="errors?.name" />
        </div>

        <div class="form-group">
          <label for="district-country">الدولة:</label>
          <select
            id="district-country"
            v-model="formData.countryID"
            class="form-select"
            :disabled="loading || countriesLoading"
            @change="onCountryChange"
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
        </div>

        <div class="form-group">
          <label class="mb-1 block font-medium">المدينة:</label>
          <GenericSearchByFilter
            v-if="formData.countryID"
            v-model="formData.cityID"
            v-model:name-value="cityName"
            :filter-types="cityFilterTypes"
            :results="cityResults"
            :loading="citySearchLoading"
            :error="citySearchError"
            id-field="id"
            name-field="displayName"
            placeholder="ابحث عن مدينة في الدولة المختارة"
            empty-message="لم يتم العثور على مدن مطابقة"
            @search="onSearchCity"
            @select="onCitySelected"
            @clear="clearCitySelection"
            @query-change="clearCitySelection"
          />
          <p v-else class="text-sm text-[var(--text-muted)]" role="status">
            اختر الدولة أولاً للبحث عن مدينة.
          </p>
          <ValidationError :messages="errors?.cityID" />
        </div>
      </fieldset>

      <p v-if="generalError" class="alert-error" role="alert">
        {{ generalError }}
      </p>

      <footer
        class="flex justify-end border-t border-[var(--border-color)] pt-4"
      >
        <button
          class="btn btn-primary"
          type="submit"
          :disabled="
            loading || countriesLoading || !formData.cityID || !selectedCity
          "
          :aria-busy="loading"
        >
          {{ loading ? "جارٍ الحفظ..." : "حفظ المنطقة" }}
        </button>
      </footer>
    </form>
  </section>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  errors: { type: [Object, String, Array], default: () => ({}) },
  countries: { type: Array, default: () => [] },
  countriesLoading: { type: Boolean, default: false },
  cityData: { type: Object, default: null },
  districtData: { type: Object, default: null },
  cityResults: { type: Array, default: () => [] },
  citySearchLoading: { type: Boolean, default: false },
  citySearchError: { type: [String, Array, Object], default: null },
  loading: { type: Boolean, default: false },
});

const emit = defineEmits(["onSubmit", "searchCity", "clearCitySearch"]);
const formData = reactive({ id: "", name: "", cityID: "", countryID: "" });
const cityName = ref("");
const selectedCity = ref(null);
const cityFilterTypes = [{ value: "Name", label: "اسم المدينة" }];

const formTitle = computed(() =>
  formData.id ? "تعديل المنطقة" : "إضافة منطقة"
);
const generalError = computed(() => {
  const value = props.errors;
  if (typeof value === "string") return value;
  if (Array.isArray(value)) return value.join("، ");
  if (!value || typeof value !== "object") return "";
  return Object.entries(value)
    .filter(([key]) => !["name", "cityID"].includes(key))
    .flatMap(([, messages]) => flattenMessages(messages))
    .join("، ");
});

function flattenMessages(value) {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
}

function cityDisplayName(city) {
  return [city?.name || city?.cityName, city?.countryName]
    .filter(Boolean)
    .join(" — ");
}

function fillCity(city) {
  if (!city || !(city.id || city.cityID || city.CityID)) return;
  const cityID = city.id || city.cityID || city.CityID;
  formData.cityID = cityID;
  formData.countryID = city.countryID || city.CountryID || formData.countryID;
  cityName.value = city.displayName || cityDisplayName(city);
  selectedCity.value = {
    ...city,
    id: cityID,
    displayName: cityName.value,
  };
}

function onCountryChange() {
  clearCitySelection();
}

function clearCitySelection() {
  formData.cityID = "";
  cityName.value = "";
  selectedCity.value = null;
  emit("clearCitySearch");
}

function onSearchCity(criteria) {
  emit("searchCity", { ...criteria, countryID: formData.countryID });
}

function onCitySelected(city) {
  fillCity(city);
}

function fillDistrict(district) {
  if (!district) {
    formData.id = "";
    formData.name = "";
    formData.cityID = "";
    formData.countryID = "";
    cityName.value = "";
    selectedCity.value = null;
    return;
  }

  formData.id = district.id || district.DistrictID || district.ID || "";
  formData.name = district.name || district.DistrictName || "";
  fillCity({
    id: district.cityID || district.CityID,
    name: district.cityName || district.CityName,
    countryID: district.countryID || district.CountryID,
    countryName: district.countryName || district.CountryName,
  });
}

function onSubmit() {
  if (props.loading || !formData.cityID || !selectedCity.value) return;
  emit("onSubmit", { ...formData });
}

watch(() => props.districtData, fillDistrict, { immediate: true, deep: true });
watch(
  () => props.cityData,
  (city) => {
    if (!props.districtData && city) fillCity(city);
  },
  { immediate: true, deep: true }
);
</script>
