<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>الاسم الأول:</label>
        <input v-model="formData.firstName" type="text" />
        <ValidationError :messages="errors?.firstName" />
      </div>

      <div class="form-group">
        <label>الكنية:</label>
        <input v-model="formData.lastName" type="text" />
        <ValidationError :messages="errors?.lastName" />
      </div>

      <div class="form-group">
        <label>اسم الأب:</label>
        <input v-model="formData.fatherName" type="text" />
        <ValidationError :messages="errors?.fatherName" />
      </div>

      <div class="form-group">
        <label>اسم الأم:</label>
        <input v-model="formData.motherName" type="text" />
        <ValidationError :messages="errors?.motherName" />
      </div>
      <div class="form-group">
        <label>كنية الأم:</label>
        <input v-model="formData.motherLastName" type="text" />
        <ValidationError :messages="errors?.motherLastName" />
      </div>
      <div class="form-group">
        <label>رقم الهوية:</label>
        <input v-model="formData.nationalNumber" type="text" />
        <ValidationError :messages="errors?.nationalNumber" />
      </div>

      <div class="form-group">
        <label>الجنس:</label>
        <input v-model="formData.gender" type="checkbox" />
        <ValidationError :messages="errors?.gender" />
      </div>

      <div class="form-group">
        <label>تاريخ الولادة:</label>
        <input v-model="formData.birthDate" type="date" />
        <ValidationError :messages="errors?.birthDate" />
      </div>

      <div class="form-group">
        <label>الدولة</label>
        <select @change="$emit('CountryChange', $event.target.value)">
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

      <div class="form-group">
        <label>المدينة</label>
        <select v-model="formData.placeOfBirthID">
          <option value="" disabled>اختر المدينة</option>
          <option v-for="city in cities" :key="city.id" :value="city.id">
            {{ city.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.placeOfBirthID" />
      </div>

      <!-- تضمين مكون العنوان الفرعي بربط v-model -->
      <fieldset class="address-section">
        <legend>بيانات العنوان</legend>
        <UpsertAddressComp
          :modelValue="formData.address"
          :errors="errors?.address"
          :countries="countries"
          :cities="cities"
          :districts="districts"
          :neighborhoods="neighborhoods"
          @CountryChange="$emit('CountryChange', $event)"
          @CityChange="$emit('CityChange', $event)"
          @DistrictChange="$emit('DistrictChange', $event)"
          @NeighboodChange="$emit('NeighboodChange', $event)"
          @updatemodelValue="fillAddress"
        />
      </fieldset>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import UpsertAddressComp from "@/components/Address/Address/upsertAddressItem.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";

export default {
  name: "UpsertPersonComp",
  components: {
    ValidationError,
    UpsertAddressComp,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    countries: { type: Array, default: () => [] },
    cities: { type: Array, default: () => [] },
    districts: { type: Array, default: () => [] },
    neighborhoods: { type: Array, default: () => [] },
    address: {
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
  },
  data() {
    return {
      personStore: usePersonStore(),
      formData: {
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
        // كائن العنوان يتصل بالـ Sub-form مباشرة
        address: {
          id: "",
          countryID: "",
          cityID: "",
          districtID: "",
          neighborhoodID: "",
          notes: "",
        },
      },
    };
  },
  emits: [
    "onSubmit",
    "CountryChange",
    "CityChange",
    "DistrictChange",
    "NeighboodChange",
  ],
  computed: {
    FormTitle() {
      return this.formData.id ? "تعديل شخص" : "إضافة شخص";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    fillSelectedPerson() {
      if (this.personStore.selectedPerson && this.address) {
        const person = this.personStore.selectedPerson;
        this.formData = {
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
          address: {
            id: this.address.id || "",
            countryID: this.address?.countryID || "",
            cityID: this.address?.cityID || "",
            districtID: this.address?.districtID || "",
            neighborhoodID: this.address?.neighborhoodID || "",
            notes: this.address?.notes || "",
          },
        };
      }
    },
    fillAddress(field) {
      this.formData.address = field;
    },
  },

  mounted() {
    this.fillSelectedPerson();
  },
};
</script>
