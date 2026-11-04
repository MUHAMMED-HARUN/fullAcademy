<template>
  <section class="space-y-4" dir="rtl" aria-label="اختيار العنوان">
    <div class="form-group">
      <label for="address-country">الدولة:</label>
      <select
        id="address-country"
        :value="modelValue.countryID"
        class="form-select"
        :disabled="loading || countriesLoading"
        :aria-invalid="!!errors?.countryID"
        @change="onCountryChange($event.target.value)"
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
      <ValidationError :messages="errors?.countryID" />
    </div>

    <div class="form-group">
      <label class="mb-1 block font-medium">المدينة:</label>
      <select
        v-if="legacyOptionsMode"
        id="address-city"
        :value="cityIDModel"
        class="form-select"
        :disabled="loading || !modelValue.countryID"
        :aria-invalid="!!errors?.cityID"
        @change="onLegacyCityChange($event.target.value)"
      >
        <option value="" disabled>اختر مدينة</option>
        <option
          v-for="city in cities || []"
          :key="city.id || city.cityID"
          :value="city.id || city.cityID"
        >
          {{ city.name || city.cityName }}
        </option>
      </select>
      <GenericSearchByFilter
        v-else-if="modelValue.countryID"
        v-model="cityIDModel"
        v-model:name-value="cityName"
        :filter-types="cityFilterTypes"
        :results="cityResults"
        :loading="citySearchLoading"
        :error="citySearchError"
        :initial-suggestions="true"
        :suggestion-key="modelValue.countryID"
        id-field="id"
        name-field="displayName"
        placeholder="ابحث عن مدينة في الدولة المختارة"
        empty-message="لم يتم العثور على مدن مطابقة"
        @search="onSearchCity"
        @select="onCitySelected"
        @clear="clearCitySelection"
      />
      <p v-else class="text-sm text-[var(--text-muted)]" role="status">
        اختر الدولة أولاً للبحث عن مدينة.
      </p>
      <ValidationError :messages="errors?.cityID" />
    </div>

    <div class="form-group">
      <label class="mb-1 block font-medium">المنطقة:</label>
      <select
        v-if="legacyOptionsMode"
        id="address-district"
        :value="districtIDModel"
        class="form-select"
        :disabled="loading || !modelValue.cityID"
        :aria-invalid="!!errors?.districtID"
        @change="onLegacyDistrictChange($event.target.value)"
      >
        <option value="" disabled>اختر منطقة</option>
        <option
          v-for="district in districts || []"
          :key="district.id || district.districtID"
          :value="district.id || district.districtID"
        >
          {{ district.name || district.districtName }}
        </option>
      </select>
      <GenericSearchByFilter
        v-else-if="modelValue.cityID"
        v-model="districtIDModel"
        v-model:name-value="districtName"
        :filter-types="districtFilterTypes"
        :results="districtResults"
        :loading="districtSearchLoading"
        :error="districtSearchError"
        :initial-suggestions="true"
        :suggestion-key="modelValue.cityID"
        id-field="id"
        name-field="displayName"
        placeholder="ابحث عن منطقة في المدينة المختارة"
        empty-message="لم يتم العثور على مناطق مطابقة"
        @search="onSearchDistrict"
        @select="onDistrictSelected"
        @clear="clearDistrictSelection"
      />
      <p v-else class="text-sm text-[var(--text-muted)]" role="status">
        اختر المدينة أولاً للبحث عن منطقة.
      </p>
      <ValidationError :messages="errors?.districtID" />
    </div>

    <div class="form-group">
      <label class="mb-1 block font-medium">الشارع:</label>
      <select
        v-if="legacyOptionsMode"
        id="address-neighborhood"
        :value="neighborhoodIDModel"
        class="form-select"
        :disabled="loading || !modelValue.districtID"
        :aria-invalid="!!errors?.neighborhoodID"
        @change="onLegacyNeighborhoodChange($event.target.value)"
      >
        <option value="" disabled>اختر شارعاً</option>
        <option
          v-for="neighborhood in neighborhoods || []"
          :key="neighborhood.id || neighborhood.neighborhoodID"
          :value="neighborhood.id || neighborhood.neighborhoodID"
        >
          {{ neighborhood.name || neighborhood.neighborhoodName }}
        </option>
      </select>
      <GenericSearchByFilter
        v-else-if="modelValue.districtID"
        v-model="neighborhoodIDModel"
        v-model:name-value="neighborhoodName"
        :filter-types="neighborhoodFilterTypes"
        :results="neighborhoodResults"
        :loading="neighborhoodSearchLoading"
        :error="neighborhoodSearchError"
        :initial-suggestions="true"
        :suggestion-key="modelValue.districtID"
        id-field="id"
        name-field="displayName"
        placeholder="ابحث عن شارع في المنطقة المختارة"
        empty-message="لم يتم العثور على شوارع مطابقة"
        @search="onSearchNeighborhood"
        @select="onNeighborhoodSelected"
        @clear="clearNeighborhoodSelection"
      />
      <p v-else class="text-sm text-[var(--text-muted)]" role="status">
        اختر المنطقة أولاً للبحث عن شارع.
      </p>
      <ValidationError :messages="errors?.neighborhoodID" />
    </div>

    <div class="form-group">
      <label for="address-notes">ملاحظات العنوان:</label>
      <input
        id="address-notes"
        :value="modelValue.notes || ''"
        class="form-control"
        type="text"
        maxlength="100"
        :disabled="loading"
        :aria-invalid="!!errors?.notes"
        @input="updateField('notes', $event.target.value)"
      />
      <ValidationError :messages="errors?.notes" />
    </div>
  </section>
</template>

<script setup>
import { computed, ref, watch } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  modelValue: {
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
  errors: { type: [Object, String, Array], default: () => ({}) },
  countries: { type: Array, default: () => [] },
  cities: { type: Array, default: undefined },
  districts: { type: Array, default: undefined },
  neighborhoods: { type: Array, default: undefined },
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
});

const emit = defineEmits([
  "update:modelValue",
  "search-city",
  "clear-city-search",
  "search-district",
  "clear-district-search",
  "search-neighborhood",
  "clear-neighborhood-search",
  "CountryChange",
  "CityChange",
  "DistrictChange",
  "NeighboodChange",
  "updatemodelValue",
]);

const legacyOptionsMode = computed(
  () =>
    props.cities !== undefined ||
    props.districts !== undefined ||
    props.neighborhoods !== undefined
);

const cityName = ref("");
const districtName = ref("");
const neighborhoodName = ref("");
const selectedCity = ref(null);
const selectedDistrict = ref(null);
const selectedNeighborhood = ref(null);
const cityFilterTypes = [{ value: "Name", label: "اسم المدينة" }];
const districtFilterTypes = [{ value: "DistrictName", label: "اسم المنطقة" }];
const neighborhoodFilterTypes = [
  { value: "NeighborhoodName", label: "اسم الشارع" },
];

const cityIDModel = computed({
  get: () => props.modelValue.cityID || "",
  set: (value) => updateField("cityID", value),
});
const districtIDModel = computed({
  get: () => props.modelValue.districtID || "",
  set: (value) => updateField("districtID", value),
});
const neighborhoodIDModel = computed({
  get: () => props.modelValue.neighborhoodID || "",
  set: (value) => updateField("neighborhoodID", value),
});

function flattenMessages(value) {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
}

const countriesErrorText = computed(() =>
  flattenMessages(props.countriesError).join("، ")
);

function updateField(field, value) {
  emitModelValue({ ...props.modelValue, [field]: value });
}

function emitModelValue(value) {
  emit("update:modelValue", value);
  if (legacyOptionsMode.value) emit("updatemodelValue", value);
}

function clearNeighborhoodSelection() {
  neighborhoodName.value = "";
  selectedNeighborhood.value = null;
  emitModelValue({
    ...props.modelValue,
    neighborhoodID: "",
    neighborhoodName: "",
    neighborhoodDisplayName: "",
  });
  emit("clear-neighborhood-search");
}

function clearDistrictSelection() {
  districtName.value = "";
  selectedDistrict.value = null;
  neighborhoodName.value = "";
  selectedNeighborhood.value = null;
  emitModelValue({
    ...props.modelValue,
    districtID: "",
    neighborhoodID: "",
    districtName: "",
    districtDisplayName: "",
    neighborhoodName: "",
    neighborhoodDisplayName: "",
  });
  emit("clear-district-search");
  emit("clear-neighborhood-search");
}

function clearCitySelection() {
  cityName.value = "";
  selectedCity.value = null;
  districtName.value = "";
  selectedDistrict.value = null;
  neighborhoodName.value = "";
  selectedNeighborhood.value = null;
  emitModelValue({
    ...props.modelValue,
    cityID: "",
    districtID: "",
    neighborhoodID: "",
    cityName: "",
    cityDisplayName: "",
    districtName: "",
    districtDisplayName: "",
    neighborhoodName: "",
    neighborhoodDisplayName: "",
  });
  emit("clear-city-search");
  emit("clear-district-search");
  emit("clear-neighborhood-search");
}

function onCountryChange(countryID) {
  emitModelValue({
    ...props.modelValue,
    countryID,
    cityID: "",
    districtID: "",
    neighborhoodID: "",
    cityName: "",
    cityDisplayName: "",
    districtName: "",
    districtDisplayName: "",
    neighborhoodName: "",
    neighborhoodDisplayName: "",
  });
  cityName.value = "";
  selectedCity.value = null;
  districtName.value = "";
  selectedDistrict.value = null;
  neighborhoodName.value = "";
  selectedNeighborhood.value = null;
  emit("clear-city-search");
  emit("clear-district-search");
  emit("clear-neighborhood-search");
  if (legacyOptionsMode.value) emit("CountryChange", countryID);
}

function onLegacyCityChange(cityID) {
  emitModelValue({
    ...props.modelValue,
    cityID,
    districtID: "",
    neighborhoodID: "",
  });
  emit("CityChange", cityID);
}

function onLegacyDistrictChange(districtID) {
  emitModelValue({ ...props.modelValue, districtID, neighborhoodID: "" });
  emit("DistrictChange", districtID);
}

function onLegacyNeighborhoodChange(neighborhoodID) {
  emitModelValue({ ...props.modelValue, neighborhoodID });
  emit("NeighboodChange", neighborhoodID);
}

function onSearchCity(criteria) {
  emit("search-city", { ...criteria, countryID: props.modelValue.countryID });
}

function onSearchDistrict(criteria) {
  emit("search-district", { ...criteria, cityID: props.modelValue.cityID });
}

function onSearchNeighborhood(criteria) {
  emit("search-neighborhood", {
    ...criteria,
    districtID: props.modelValue.districtID,
  });
}

function onCitySelected(city) {
  selectedCity.value = city;
  cityName.value = city.displayName || city.name || "";
  emit("update:modelValue", {
    ...props.modelValue,
    cityID: city.id,
    districtID: "",
    neighborhoodID: "",
    cityName: city.name || "",
    cityDisplayName: city.displayName || city.name || "",
    districtName: "",
    districtDisplayName: "",
    neighborhoodName: "",
    neighborhoodDisplayName: "",
  });
  districtName.value = "";
  selectedDistrict.value = null;
  neighborhoodName.value = "";
  selectedNeighborhood.value = null;
  emit("clear-district-search");
  emit("clear-neighborhood-search");
}

function onDistrictSelected(district) {
  selectedDistrict.value = district;
  districtName.value = district.displayName || district.name || "";
  emit("update:modelValue", {
    ...props.modelValue,
    districtID: district.id,
    neighborhoodID: "",
    districtName: district.name || "",
    districtDisplayName: district.displayName || district.name || "",
    neighborhoodName: "",
    neighborhoodDisplayName: "",
  });
  neighborhoodName.value = "";
  selectedNeighborhood.value = null;
  emit("clear-neighborhood-search");
}

function onNeighborhoodSelected(neighborhood) {
  selectedNeighborhood.value = neighborhood;
  neighborhoodName.value = neighborhood.displayName || neighborhood.name || "";
  emit("update:modelValue", {
    ...props.modelValue,
    neighborhoodID: neighborhood.id,
    neighborhoodName: neighborhood.name || "",
    neighborhoodDisplayName:
      neighborhood.displayName || neighborhood.name || "",
  });
}

watch(
  () => props.modelValue,
  (address) => {
    cityName.value = address.cityDisplayName || address.cityName || "";
    districtName.value =
      address.districtDisplayName || address.districtName || "";
    neighborhoodName.value =
      address.neighborhoodDisplayName || address.neighborhoodName || "";
    selectedCity.value = address.cityID
      ? { id: address.cityID, displayName: cityName.value }
      : null;
    selectedDistrict.value = address.districtID
      ? { id: address.districtID, displayName: districtName.value }
      : null;
    selectedNeighborhood.value = address.neighborhoodID
      ? { id: address.neighborhoodID, displayName: neighborhoodName.value }
      : null;
  },
  { immediate: true, deep: true }
);
</script>
