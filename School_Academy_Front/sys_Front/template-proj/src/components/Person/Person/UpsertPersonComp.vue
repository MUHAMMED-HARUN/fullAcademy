<template>
  <div>
    <div>{{ this.FormTitle }}</div>
    <form @submit.prevent.stop="onSubmit">
      <div class="form-group">
        <label>اسم:</label>
        <input v-model="formData.fatherName" type="text" />
        <ValidationError :messages="errors?.fatherName" />
      </div>
      <div class="form-group">
        <label>الكنية:</label>
        <input v-model="formData.lastName" type="text" />
        <ValidationError :messages="errors?.lastName" />
      </div>
      <div class="form-group">
        <label>اسم الاب:</label>
        <input v-model="formData.fatherName" type="text" />
        <ValidationError :messages="errors?.fatherName" />
      </div>
      <div class="form-group">
        <label>اسم الام:</label>
        <input v-model="formData.motherName" type="text" />
        <ValidationError :messages="errors?.motherName" />
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
      <UpsertAddressComp />
      <div class="form-group">
        <button>حفظ</button>
      </div>
    </form>
  </div>
</template>
<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import UpsertAddressComp from "@/components/Address/Address/UpsertAddressComp.vue";
export default {
  name: "UpsertPersonComp",
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
      PersonStore: usePersonStore(),
      formData: {
        id: "",
        firstName: "",
        fatherName: "",
        lastName: "",
        motherName: "",
        motherLastName: "",
        nationalNumber: "",
        gender: true,
        birthDate: new Date(2000, 1, 1),
        placeOfBirthID: "",
        livingPlaceID: "",
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
    UpsertAddressComp,
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
