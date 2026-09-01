<template>
  <div class="address-form-section">
    <!-- الدولة -->
    <div class="form-group">
      <label>الدولة</label>
      <select
        :value="modelValue.countryID"
        @change="
          updateField('countryID', $event.target.value);
          $emit('CountryChange', $event.target.value);
        "
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
      <ValidationError :messages="errors?.countryID" />
    </div>

    <!-- المدينة -->
    <div class="form-group">
      <label>المدينة</label>
      <select
        :value="modelValue.cityID"
        @change="
          updateField('cityID', $event.target.value);
          $emit('CityChange', $event.target.value);
        "
      >
        <option value="" disabled>اختر مدينة</option>
        <option v-for="city in cities" :key="city.id" :value="city.id">
          {{ city.name }}
        </option>
      </select>
      <ValidationError :messages="errors?.cityID" />
    </div>

    <!-- الحارة / الحي -->
    <div class="form-group">
      <label>الحارة</label>
      <select
        :value="modelValue.districtID"
        @change="
          updateField('districtID', $event.target.value);
          $emit('DistrictChange', $event.target.value);
        "
      >
        <option value="" disabled>اختر حارة</option>
        <option
          v-for="district in districts"
          :key="district.id"
          :value="district.id"
        >
          {{ district.name }}
        </option>
      </select>
      <ValidationError :messages="errors?.districtID" />
    </div>

    <!-- الشارع -->
    <div class="form-group">
      <label>الشارع</label>
      <select
        :value="modelValue.neighborhoodID"
        @change="
          updateField('neighborhoodID', $event.target.value);
          $emit('NeighboodChange', $event.target.value);
        "
      >
        <option value="" disabled>اختر شارع</option>
        <option
          v-for="neighborhood in neighborhoods"
          :key="neighborhood.id"
          :value="neighborhood.id"
        >
          {{ neighborhood.name }}
        </option>
      </select>
      <ValidationError :messages="errors?.neighborhoodID" />
    </div>

    <!-- ملاحظات العنوان -->
    <div class="form-group">
      <label>ملاحظات العنوان:</label>
      <input
        :value="modelValue.notes"
        @input="updateField('notes', $event.target.value)"
        type="text"
      />
      <ValidationError :messages="errors?.notes" />
    </div>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";

export default {
  name: "UpsertAddressComp",
  components: {
    ValidationError,
  },
  props: {
    // يستقبل كائن العنوان الكامل عبر v-model
    modelValue: {
      type: Object,
      default: () => ({
        countryID: "",
        cityID: "",
        districtID: "",
        neighborhoodID: "",
        notes: "",
      }),
    },
    errors: {
      type: Object,
      default: () => ({}),
    },
    countries: {
      type: Array,
      default: () => [],
    },
    cities: {
      type: Array,
      default: () => [],
    },
    districts: {
      type: Array,
      default: () => [],
    },
    neighborhoods: {
      type: Array,
      default: () => [],
    },
  },
  emits: [
    "updatemodelValue",
    "CountryChange",
    "CityChange",
    "DistrictChange",
    "NeighboodChange",
  ],
  methods: {
    updateField(field, value) {
      this.$emit("updatemodelValue", {
        ...this.modelValue,
        [field]: value,
      });
    },
  },
};
</script>
