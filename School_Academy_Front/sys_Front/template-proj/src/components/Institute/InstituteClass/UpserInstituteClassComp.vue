<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>المعهد:</label>
        <GenericSearchByFilter
          :filter-types="instituteFilterTypes"
          :results="instituteResults"
          id-field="instituteID"
          name-field="name"
          placeholder="ابحث عن معهد"
          :loading="instituteSearchLoading"
          :error="instituteSearchError"
          v-model="formData.instituteID"
          v-model:name-value="instituteName"
          @search="onSearchInstitute"
          @select="onInstituteSelected"
          @clear="clearInstituteSelection"
        />
        <ValidationError :messages="errors?.instituteID" />
        <InstituteCard
          v-if="showInstituteCard"
          :data-req="selectedInstituteData"
        />
      </div>

      <div class="form-group">
        <label>الصف:</label>
        <GenericSearchByFilter
          :filter-types="classFilterTypes"
          :results="classResults"
          id-field="classID"
          name-field="name"
          placeholder="ابحث عن صف"
          :loading="isSearchingClass"
          v-model="formData.classID"
          v-model:name-value="className"
          @search="onSearchClass"
          @select="onClassSelected"
          @clear="clearClassSelection"
        />
        <ValidationError :messages="errors?.classID" />
        <ClassCard v-if="showClassCard" :data-req="selectedClassData" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ClassCard from "../Class/ClassCard.vue";
import InstituteCard from "../Institute/InstituteCard.vue";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore.js";
import { useClassStore } from "@/store/Institute/ClassStore";

export default {
  name: "UpsertInstituteClassComp",
  components: {
    ValidationError,
    GenericSearchByFilter,
    ClassCard,
    InstituteCard,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    InstituteData: {
      type: Object,
      default: () => ({}),
    },
    instituteResults: { type: Array, default: () => [] },
    instituteSearchLoading: { type: Boolean, default: false },
    instituteSearchError: { type: [String, Array, Object], default: null },
    ClassData: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      InstituteClassStore: useInstituteClassStore(),
      ClassStore: useClassStore(),
      formData: {
        instituteClassID: "",
        classID: "",
        instituteID: "",
      },
      instituteFilterTypes: [{ value: 1, label: "بحث باسم المعهد" }],
      classFilterTypes: [{ value: 1, label: "بحث باسم الصف" }],
      classResults: [],
      instituteName: "",
      className: "",
      selectedInstituteData: null,
      selectedClassData: null,
      isSearchingClass: false,
      showInstituteCard: false,
      showClassCard: false,
    };
  },
  emits: ["onSubmit", "search-institute", "clear-institute-search"],
  computed: {
    FormTitle() {
      return this.formData.instituteClassID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onSearchInstitute(criteria) {
      this.$emit("search-institute", criteria);
    },
    async onSearchClass(payload) {
      this.isSearchingClass = true;
      await this.ClassStore.fetchClasses();
      const q = String(payload.value || "").toLowerCase();
      this.classResults = (this.ClassStore.classes || []).filter((c) =>
        String(c.name || "")
          .toLowerCase()
          .includes(q)
      );
      this.isSearchingClass = false;
    },
    onInstituteSelected(item) {
      this.formData.instituteID = item.instituteID;
      this.instituteName = item.name;
      this.selectedInstituteData = item;
      this.showInstituteCard = true;
    },
    onClassSelected(item) {
      this.formData.classID = item.classID;
      this.className = item.name;
      this.selectedClassData = item;
      this.showClassCard = true;
    },
    clearInstituteSelection() {
      this.formData.instituteID = "";
      this.instituteName = "";
      this.selectedInstituteData = null;
      this.showInstituteCard = false;
      this.$emit("clear-institute-search");
    },
    clearClassSelection() {
      this.selectedClassData = null;
      this.showClassCard = false;
    },
    fillSelectedInstituteClass() {
      if (this.InstituteClassStore.selectedInstituteClass) {
        const entity = this.InstituteClassStore.selectedInstituteClass;
        this.formData = {
          instituteClassID: entity.instituteClassID || "",
          classID: entity.classID || "",
          instituteID: entity.instituteID || "",
        };
      }
    },
    handleClassData(data, isFound) {
      if (isFound) {
        this.formData.classID = data.classID;
        this.selectedClassData = data;
        this.showClassCard = true;
      } else {
        this.selectedClassData = null;
        this.showClassCard = false;
      }
    },
    handleInstituteData(data, isFound) {
      if (isFound) {
        this.formData.instituteID = data.instituteID;
        this.selectedInstituteData = data;
        this.showInstituteCard = true;
      } else {
        this.selectedInstituteData = null;
        this.showInstituteCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedInstituteClass();
  },
  watch: {
    ClassData: {
      handler(data) {
        if (data && data.classID != null) {
          this.handleClassData(data, true);
        } else {
          this.handleClassData(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
    InstituteData: {
      handler(data) {
        if (data && data.instituteID != null) {
          this.handleInstituteData(data, true);
        } else {
          this.handleInstituteData(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
