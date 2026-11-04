<template>
  <section class="space-y-5" dir="rtl">
    <header class="space-y-1">
      <h2 class="text-xl font-bold text-[var(--primary-color)]">
        {{ formTitle }}
      </h2>
      <p class="text-sm text-[var(--text-muted)]">
        أدخل بيانات الشخص وعنوانه. خيارات العنوان تُحمّل حسب الاختيار.
      </p>
    </header>

    <form class="space-y-5" novalidate @submit.prevent="onSubmit">
      <fieldset
        class="m-0 min-w-0 space-y-5 border-0 p-0"
        :disabled="props.loading"
      >
        <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div class="form-group min-w-0">
            <label for="person-first-name">الاسم الأول:</label>
            <input
              id="person-first-name"
              v-model="formData.firstName"
              class="form-control"
              type="text"
              autocomplete="given-name"
              :aria-invalid="!!props.errors?.firstName"
              aria-required="true"
            />
            <ValidationError :messages="props.errors?.firstName" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-father-name">اسم الأب:</label>
            <input
              id="person-father-name"
              v-model="formData.fatherName"
              class="form-control"
              type="text"
              :aria-invalid="!!props.errors?.fatherName"
              aria-required="true"
            />
            <ValidationError :messages="props.errors?.fatherName" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-last-name">الكنية:</label>
            <input
              id="person-last-name"
              v-model="formData.lastName"
              class="form-control"
              type="text"
              autocomplete="family-name"
              :aria-invalid="!!props.errors?.lastName"
              aria-required="true"
            />
            <ValidationError :messages="props.errors?.lastName" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-mother-name">اسم الأم:</label>
            <input
              id="person-mother-name"
              v-model="formData.motherName"
              class="form-control"
              type="text"
              :aria-invalid="!!props.errors?.motherName"
              aria-required="true"
            />
            <ValidationError :messages="props.errors?.motherName" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-mother-last-name">كنية الأم:</label>
            <input
              id="person-mother-last-name"
              v-model="formData.motherLastName"
              class="form-control"
              type="text"
              :aria-invalid="!!props.errors?.motherLastName"
              aria-required="true"
            />
            <ValidationError :messages="props.errors?.motherLastName" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-national-number">الرقم الوطني:</label>
            <input
              id="person-national-number"
              v-model="formData.nationalNumber"
              class="form-control"
              type="text"
              autocomplete="off"
              :aria-invalid="!!props.errors?.nationalNumber"
              aria-required="true"
            />
            <ValidationError :messages="props.errors?.nationalNumber" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-gender">الجنس:</label>
            <select
              id="person-gender"
              v-model="formData.gender"
              class="form-select"
            >
              <option :value="true">ذكر</option>
              <option :value="false">أنثى</option>
            </select>
            <ValidationError :messages="props.errors?.gender" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-birth-date">تاريخ الولادة:</label>
            <input
              id="person-birth-date"
              v-model="formData.birthDate"
              class="form-control"
              type="date"
              :aria-invalid="!!props.errors?.birthDate"
              aria-required="true"
            />
            <ValidationError :messages="props.errors?.birthDate" />
          </div>

          <div class="form-group min-w-0">
            <label for="person-birth-country">دولة الولادة:</label>
            <select
              id="person-birth-country"
              v-model="birthCountryID"
              class="form-select"
              :disabled="props.loading || props.countriesLoading"
              @change="onBirthCountryChange"
            >
              <option value="" disabled>اختر دولة</option>
              <option
                v-for="country in props.countries"
                :key="country.ID || country.id"
                :value="country.ID || country.id"
              >
                {{ country.Name || country.name }}
              </option>
            </select>
            <ValidationError :messages="props.errors?.placeOfBirthID" />
          </div>

          <div class="form-group min-w-0">
            <label class="mb-1 block font-medium">مدينة الولادة:</label>
            <GenericSearchByFilter
              v-if="birthCountryID"
              v-model="birthCityID"
              v-model:name-value="birthCityName"
              :filter-types="cityFilterTypes"
              :results="props.birthCityResults"
              :loading="props.birthCitySearchLoading"
              :error="props.birthCitySearchError"
              id-field="id"
              name-field="displayName"
              placeholder="ابحث عن مدينة الولادة"
              empty-message="لم يتم العثور على مدن مطابقة"
              @search="onSearchBirthCity"
              @select="onBirthCitySelected"
              @clear="clearBirthCity"
              @query-change="clearBirthCity"
            />
            <p v-else class="text-sm text-[var(--text-muted)]" role="status">
              اختر الدولة أولاً للبحث عن مدينة الولادة.
            </p>
          </div>
        </div>

        <fieldset
          class="space-y-3 rounded-xl border border-[var(--border-color)] bg-[var(--bg-body)] p-4"
        >
          <legend class="px-2 font-semibold text-[var(--primary-color)]">
            بيانات العنوان
          </legend>
          <UpsertAddressComp
            v-model="formData.address"
            :errors="props.errors?.address"
            :countries="props.countries"
            :countries-loading="props.countriesLoading"
            :countries-error="props.countriesError"
            :city-results="props.addressCityResults"
            :city-search-loading="props.addressCitySearchLoading"
            :city-search-error="props.addressCitySearchError"
            :district-results="props.addressDistrictResults"
            :district-search-loading="props.addressDistrictSearchLoading"
            :district-search-error="props.addressDistrictSearchError"
            :neighborhood-results="props.addressNeighborhoodResults"
            :neighborhood-search-loading="
              props.addressNeighborhoodSearchLoading
            "
            :neighborhood-search-error="props.addressNeighborhoodSearchError"
            :loading="props.loading"
            @search-city="emit('search-address-city', $event)"
            @clear-city-search="emit('clear-address-city-search')"
            @search-district="emit('search-address-district', $event)"
            @clear-district-search="emit('clear-address-district-search')"
            @search-neighborhood="emit('search-address-neighborhood', $event)"
            @clear-neighborhood-search="
              emit('clear-address-neighborhood-search')
            "
          />
        </fieldset>
      </fieldset>

      <div v-if="props.errors?._form" role="alert">
        <ValidationError :messages="props.errors._form" />
      </div>

      <div class="flex justify-end border-t border-[var(--border-color)] pt-4">
        <button
          class="btn btn-primary min-w-32"
          type="submit"
          :disabled="props.loading"
          :aria-busy="props.loading"
        >
          <span
            v-if="props.loading"
            class="inline-block h-4 w-4 animate-spin rounded-full border-2 border-white/40 border-t-white"
            aria-hidden="true"
          />
          {{ props.loading ? "جارٍ الحفظ..." : "حفظ البيانات" }}
        </button>
      </div>
    </form>
  </section>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ValidationError from "@/components/general/ValidationError.vue";
import UpsertAddressComp from "@/components/Address/Address/upsertAddressItem.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";

const props = defineProps({
  errors: { type: [Object, String, Array], default: () => ({}) },
  countries: { type: Array, default: () => [] },
  countriesLoading: { type: Boolean, default: false },
  countriesError: { type: [String, Array, Object], default: null },
  birthCityData: { type: Object, default: null },
  birthCityResults: { type: Array, default: () => [] },
  birthCitySearchLoading: { type: Boolean, default: false },
  birthCitySearchError: { type: [String, Array, Object], default: null },
  addressCityResults: { type: Array, default: () => [] },
  addressCitySearchLoading: { type: Boolean, default: false },
  addressCitySearchError: { type: [String, Array, Object], default: null },
  addressDistrictResults: { type: Array, default: () => [] },
  addressDistrictSearchLoading: { type: Boolean, default: false },
  addressDistrictSearchError: { type: [String, Array, Object], default: null },
  addressNeighborhoodResults: { type: Array, default: () => [] },
  addressNeighborhoodSearchLoading: { type: Boolean, default: false },
  addressNeighborhoodSearchError: {
    type: [String, Array, Object],
    default: null,
  },
  address: { type: Object, default: null },
  loading: { type: Boolean, default: false },
});

const emit = defineEmits([
  "onSubmit",
  "search-birth-city",
  "clear-birth-city-search",
  "search-address-city",
  "clear-address-city-search",
  "search-address-district",
  "clear-address-district-search",
  "search-address-neighborhood",
  "clear-address-neighborhood-search",
]);

const personStore = usePersonStore();
const birthCountryID = ref("");
const birthCityName = ref("");
const selectedBirthCity = ref(null);
const cityFilterTypes = [{ value: "Name", label: "اسم المدينة" }];

const emptyAddress = () => ({
  id: "",
  countryID: "",
  cityID: "",
  districtID: "",
  neighborhoodID: "",
  notes: "",
  countryName: "",
  cityName: "",
  districtName: "",
  neighborhoodName: "",
});

const emptyPerson = () => ({
  id: "",
  firstName: "",
  fatherName: "",
  lastName: "",
  motherName: "",
  motherLastName: "",
  nationalNumber: "",
  gender: true,
  birthDate: null,
  placeOfBirthID: "",
  livingPlaceID: "",
  address: emptyAddress(),
});

const formData = reactive(emptyPerson());
const formTitle = computed(() =>
  formData.id ? "تعديل بيانات شخص" : "إضافة شخص جديد"
);
const birthCityID = computed({
  get: () => formData.placeOfBirthID,
  set: (value) => {
    formData.placeOfBirthID = value || "";
  },
});

function birthCityLabel(city) {
  return [city?.name || city?.cityName, city?.countryName]
    .filter(Boolean)
    .join(" — ");
}

function syncBirthCity(city) {
  if (!city || !(city.id || city.cityID)) {
    birthCountryID.value = "";
    birthCityName.value = "";
    selectedBirthCity.value = null;
    return;
  }

  const id = city.id || city.cityID;
  formData.placeOfBirthID = id;
  birthCountryID.value = city.countryID || "";
  birthCityName.value = city.displayName || birthCityLabel(city);
  selectedBirthCity.value = { ...city, id, displayName: birthCityName.value };
}

function fillSelectedPerson() {
  const person = personStore.selectedPerson;
  if (!person) {
    Object.assign(formData, emptyPerson());
    birthCountryID.value = "";
    birthCityName.value = "";
    selectedBirthCity.value = null;
    return;
  }

  const address = props.address || person.address || emptyAddress();
  Object.assign(formData, {
    id: person.id || "",
    firstName: person.firstName || "",
    fatherName: person.fatherName || "",
    lastName: person.lastName || "",
    motherName: person.motherName || "",
    motherLastName: person.motherLastName || "",
    nationalNumber: person.nationalNumber || "",
    gender: person.gender !== undefined ? person.gender : true,
    birthDate: person.birthDate || null,
    placeOfBirthID: person.placeOfBirthID || "",
    livingPlaceID: person.livingPlaceID || "",
    address: { ...emptyAddress(), ...address },
  });
  if (props.birthCityData) syncBirthCity(props.birthCityData);
}

function onBirthCountryChange() {
  formData.placeOfBirthID = "";
  birthCityName.value = "";
  selectedBirthCity.value = null;
  emit("clear-birth-city-search");
}

function clearBirthCity() {
  formData.placeOfBirthID = "";
  birthCityName.value = "";
  selectedBirthCity.value = null;
  emit("clear-birth-city-search");
}

function onSearchBirthCity(criteria) {
  emit("search-birth-city", {
    ...criteria,
    countryID: birthCountryID.value,
  });
}

function onBirthCitySelected(city) {
  syncBirthCity(city);
}

function onSubmit() {
  emit("onSubmit", {
    ...formData,
    address: { ...formData.address },
  });
}

watch(
  () => [personStore.selectedPerson, props.address],
  () => fillSelectedPerson(),
  { immediate: true, deep: true }
);

watch(
  () => props.birthCityData,
  (city) => {
    if (city && city.id === formData.placeOfBirthID) syncBirthCity(city);
  },
  { immediate: true, deep: true }
);
</script>
