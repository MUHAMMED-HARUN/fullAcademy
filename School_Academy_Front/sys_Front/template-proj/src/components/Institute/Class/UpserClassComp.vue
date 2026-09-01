<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم الصف:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>
      <div class="form-group">
        <label>مستوى الصف:</label>
        <input v-model="formData.level" type="number" />
        <ValidationError :messages="errors?.level" />
      </div>
      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>
      <div class="form-filter">
        <label>نوع البحث</label>
        <select v-model="searchType">
          <option selected :value="this.enSearchType.ProgramName">
            بحث باسم المشروع
          </option>
          <option disabled :value="enSearchType.ProgramID">
            بحث بـ ID المشروع
          </option>
        </select>
        <input v-model="searchValue" type="text" />
        <button @click.prevent.stop="onSearchProgram">بحث</button>
      </div>

      <div class="program-card">
        <ProgramCard v-if="showCard" :-data-req="programData" />
        <ValidationError :messages="errors?.programID" />
      </div>
      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useClassStore } from "@/store/Institute/ClassStore.js";
import ProgramCard from "../Program/ProgramCard.vue";
const enSearchType = Object.freeze({ ProgramName: 1, ProgramID: 0 });

export default {
  name: "UpsertClassComp",
  components: {
    ValidationError,
    ProgramCard,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    programData: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      ClassStore: useClassStore(),
      formData: {
        classID: "",
        name: "",
        notes: "",
        level: 1,
        programID: "",
      },
      enSearchType: enSearchType,
      searchType: enSearchType.nationalNumber,
      searchValue: "",
      showCard: false,
    };
  },
  emits: ["onSubmit", "onSearchProgram"],
  computed: {
    FormTitle() {
      return this.formData.classID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onSearchProgram() {
      this.$emit("onSearchProgram", this.searchType, this.searchValue);
    },

    fillSelectedClass() {
      if (this.ClassStore.selectedClass) {
        const entity = this.ClassStore.selectedClass;
        this.formData = {
          classID: entity.classID || "",
          name: entity.name || "",
          notes: entity.notes || "",
          level: entity.level || "",
          programID: entity.programID || "",
        };
      }
    },
    handle(data, isFound) {
      if (isFound) {
        this.formData.programID = data.programID;
        this.showCard = true;
      } else {
        this.showCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedClass();
  },
  watch: {
    programData: {
      handler(data) {
        console.log("prog data", data);
        // تصحيح المنطق: التحقق من وجود البيانات مرة واحدة
        if (data.programID != null) {
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
