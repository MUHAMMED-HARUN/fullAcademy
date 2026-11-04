<template>
  <section
    class="space-y-5"
    dir="rtl"
    aria-labelledby="neighborhood-form-title"
  >
    <header>
      <h2
        id="neighborhood-form-title"
        class="text-xl font-bold text-[var(--primary-color)]"
      >
        {{ formTitle }}
      </h2>
      <p class="mt-1 text-sm text-[var(--text-muted)]">
        اختر الدولة والمدينة ثم المنطقة المرتبطة بها.
      </p>
    </header>

    <form class="space-y-4" @submit.prevent="onSubmit">
      <fieldset class="space-y-4 border-0 p-0" :disabled="loading">
        <div class="form-group">
          <label for="neighborhood-name">اسم الشارع:</label>
          <input
            id="neighborhood-name"
            v-model="formData.name"
            class="form-control"
            type="text"
          />
          <ValidationError :messages="errors?.name" />
        </div>

        <div class="form-group">
          <label for="neighborhood-country">الدولة:</label>
          <select
            id="neighborhood-country"
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
          <p v-else-if="countriesErrorText" class="alert-error" role="alert">
            {{ countriesErrorText }}
          </p>
          <p
            v-else-if="countries.length === 0"
            class="text-sm text-[var(--text-muted)]"
            role="status"
          >
            لا توجد دول متاحة للاختيار.
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
        </div>

        <div class="form-group">
          <label class="mb-1 block font-medium">المنطقة:</label>
          <GenericSearchByFilter
            v-if="formData.cityID"
            v-model="formData.districtID"
            v-model:name-value="districtName"
            :filter-types="districtFilterTypes"
            :results="districtResults"
            :loading="districtSearchLoading"
            :error="districtSearchError"
            id-field="id"
            name-field="displayName"
            placeholder="ابحث عن منطقة في المدينة المختارة"
            empty-message="لم يتم العثور على مناطق مطابقة"
            @search="onSearchDistrict"
            @select="onDistrictSelected"
            @clear="clearDistrictSelection"
            @query-change="clearDistrictSelection"
          />
          <p v-else class="text-sm text-[var(--text-muted)]" role="status">
            اختر المدينة أولاً للبحث عن منطقة.
          </p>
          <ValidationError :messages="errors?.districtID" />
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
            loading ||
            countriesLoading ||
            !formData.districtID ||
            !selectedDistrict
          "
          :aria-busy="loading"
        >
          {{ loading ? "جارٍ الحفظ..." : "حفظ الشارع" }}
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
  countriesError: { type: [String, Array, Object], default: null },
  neighborhoodData: { type: Object, default: null },
  districtData: { type: Object, default: null },
  cityResults: { type: Array, default: () => [] },
  citySearchLoading: { type: Boolean, default: false },
  citySearchError: { type: [String, Array, Object], default: null },
  districtResults: { type: Array, default: () => [] },
  districtSearchLoading: { type: Boolean, default: false },
  districtSearchError: { type: [String, Array, Object], default: null },
  loading: { type: Boolean, default: false },
});

const emit = defineEmits([
  "onSubmit",
  "search-city",
  "clear-city-search",
  "search-district",
  "clear-district-search",
]);

const formData = reactive({
  id: "",
  name: "",
  countryID: "",
  cityID: "",
  districtID: "",
});
const cityName = ref("");
const districtName = ref("");
const selectedCity = ref(null);
const selectedDistrict = ref(null);
const cityFilterTypes = [{ value: "Name", label: "اسم المدينة" }];
const districtFilterTypes = [{ value: "DistrictName", label: "اسم المنطقة" }];

const formTitle = computed(() => (formData.id ? "تعديل الشارع" : "إضافة شارع"));
const generalError = computed(() => {
  const value = props.errors;
  if (typeof value === "string") return value;
  if (Array.isArray(value)) return value.join("، ");
  if (!value || typeof value !== "object") return "";
  return Object.entries(value)
    .filter(([key]) => !["name", "districtID"].includes(key))
    .flatMap(([, messages]) => flattenMessages(messages))
    .join("، ");
});
const countriesErrorText = computed(() =>
  flattenMessages(props.countriesError).join("، ")
);

function flattenMessages(value) {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
}

function displayCity(city) {
  return [city?.name || city?.cityName, city?.countryName]
    .filter(Boolean)
    .join(" — ");
}

function displayDistrict(district) {
  return [
    district?.name || district?.districtName,
    district?.cityName,
    district?.countryName,
  ]
    .filter(Boolean)
    .join(" — ");
}

function fillCity(city) {
  if (!city || !(city.id || city.cityID || city.CityID)) return;
  const id = city.id || city.cityID || city.CityID;
  formData.cityID = id;
  formData.countryID = city.countryID || city.CountryID || formData.countryID;
  cityName.value = city.displayName || displayCity(city);
  selectedCity.value = { ...city, id, displayName: cityName.value };
}

function fillDistrict(district) {
  if (!district || !(district.id || district.districtID || district.DistrictID))
    return;
  const id = district.id || district.districtID || district.DistrictID;
  formData.districtID = id;
  formData.cityID = district.cityID || district.CityID || formData.cityID;
  formData.countryID =
    district.countryID || district.CountryID || formData.countryID;
  districtName.value = district.displayName || displayDistrict(district);
  selectedDistrict.value = { ...district, id, displayName: districtName.value };
}

function clearDistrictSelection() {
  formData.districtID = "";
  districtName.value = "";
  selectedDistrict.value = null;
  emit("clear-district-search");
}

function clearCitySelection() {
  formData.cityID = "";
  cityName.value = "";
  selectedCity.value = null;
  clearDistrictSelection();
  emit("clear-city-search");
}

function onCountryChange() {
  clearCitySelection();
}

function onSearchCity(criteria) {
  emit("search-city", { ...criteria, countryID: formData.countryID });
}

function onSearchDistrict(criteria) {
  emit("search-district", { ...criteria, cityID: formData.cityID });
}

function onCitySelected(city) {
  fillCity(city);
  clearDistrictSelection();
}

function onDistrictSelected(district) {
  fillDistrict(district);
}

function resetForm() {
  formData.id = "";
  formData.name = "";
  formData.countryID = "";
  formData.cityID = "";
  formData.districtID = "";
  cityName.value = "";
  districtName.value = "";
  selectedCity.value = null;
  selectedDistrict.value = null;
  emit("clear-city-search");
  emit("clear-district-search");
}

function fillFromNeighborhood(neighborhood) {
  if (!neighborhood) return;
  formData.id = neighborhood.id || neighborhood.neighborhoodID || "";
  formData.name = neighborhood.name || neighborhood.neighborhoodName || "";
  formData.countryID = neighborhood.countryID || "";
  formData.cityID = neighborhood.cityID || "";
  formData.districtID = neighborhood.districtID || "";
  fillCity({
    id: formData.cityID,
    name: neighborhood.cityName,
    countryID: formData.countryID,
    countryName: neighborhood.countryName,
  });
  fillDistrict({
    id: formData.districtID,
    name: neighborhood.districtName,
    cityID: formData.cityID,
    cityName: neighborhood.cityName,
    countryID: formData.countryID,
    countryName: neighborhood.countryName,
  });
}

function fillFromDistrict(district) {
  if (!district) return;
  formData.countryID = district.countryID || district.CountryID || "";
  fillCity({
    id: district.cityID || district.CityID,
    name: district.cityName || district.CityName,
    countryID: formData.countryID,
    countryName: district.countryName || district.CountryName,
  });
  fillDistrict(district);
}

function onSubmit() {
  if (
    props.loading ||
    !formData.countryID ||
    !formData.cityID ||
    !formData.districtID ||
    !selectedCity.value ||
    !selectedDistrict.value
  ) {
    return;
  }
  emit("onSubmit", {
    id: formData.id,
    name: formData.name,
    districtID: formData.districtID,
  });
}

watch(
  () => props.neighborhoodData,
  (neighborhood) => {
    if (neighborhood) fillFromNeighborhood(neighborhood);
    else if (!props.districtData) resetForm();
  },
  { immediate: true, deep: true }
);

watch(
  () => props.districtData,
  (district) => {
    if (district) fillFromDistrict(district);
    else if (!props.neighborhoodData) resetForm();
  },
  { immediate: true, deep: true }
);
</script>
