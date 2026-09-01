<template>
  <div class="student-form">
    <h3>بيانات الطالب</h3>
    <div class="form-filter">
      <label>نوع البحث</label>
      <select v-model="searchType">
        <option selected :value="this.enSearchType.nationalNumber">
          بحث برقم الهوية
        </option>
        <option disabled :value="enSearchType.personID">بحث بـ ID الشخص</option>
      </select>
      <input v-model="searchValue" type="text" />
      <button @click="searchStudent">بحث</button>
    </div>
    <div class="person-card">
      <PersonCard v-if="showCard" :-data-req="personData" />
      <ValidationError :messages="errors?.personID" />
    </div>
    <button @click="onSubmit">حفظ</button>
  </div>
</template>

<script>
import PersonCard from "../Person/PersonCard.vue";
import ValidationError from "@/components/general/ValidationError.vue";
const enSearchType = Object.freeze({ nationalNumber: 2, personID: 1 });
export default {
  name: "UpsertStudentComp",
  components: {
    ValidationError,
    PersonCard,
  },
  data() {
    return {
      enSearchType: enSearchType,
      searchType: enSearchType.nationalNumber,
      searchValue: "",
      showCard: false,
    };
  },
  props: {
    // البادئة تمكننا من إعادة استخدام الفورم بأي مسار (address, headquarters, etc.)
    modelValue: {
      type: Object,
      default: () => ({
        studentID: "",
        personID: "",
      }),
    },
    errors: {
      type: Object,
      default: () => ({}),
    },
    personData: {
      type: Object,
      default: () => ({}),
    },
  },
  emits: ["updatemodelValue", "searchStudent", "onSubmit"],
  methods: {
    updateField(field, value) {
      this.$emit("updatemodelValue", {
        ...this.modelValue,
        [field]: value,
      });
    },
    searchStudent() {
      this.$emit("searchStudent", this.searchType, this.searchValue);
    },
    onSubmit() {
      this.$emit("onSubmit", this.modelValue);
    },
    handle(data, isFound) {
      if (isFound) {
        this.updateField("personID", data.id);
        this.showCard = true;
      } else {
        this.showCard = true;
      }
    },
  },
  watch: {
    personData: {
      handler(data) {
        // تصحيح المنطق: التحقق من وجود البيانات مرة واحدة
        if (data && Object.keys(data).length > 0) {
          this.handle(data, true);
        } else {
          this.handle(null, false);
        }
      },
      immediate: true,
    },
  },
};
</script>
