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
        <input v-model.number="formData.level" type="number" />
        <ValidationError :messages="errors?.level" />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>

      <div class="form-group">
        <label>البرنامج:</label>
        <GenericSearchByFilter
          :filter-types="programFilterTypes"
          :results="programResults"
          id-field="programID"
          name-field="name"
          placeholder="ابحث عن برنامج"
          :loading="isSearching"
          v-model:model-value="formData.programID"
          v-model:name-value="programName"
          @search="onSearchProgram"
          @select="onProgramSelected"
          @clear="clearProgramSelection"
        />
        <ValidationError :messages="errors?.programID" />
        <ProgramCard v-if="showCard" :-data-req="programDataCard" />
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
import ProgramCard from "../Program/ProgramCard.vue";
import { useClassStore } from "@/store/Institute/ClassStore.js";
import { useProgramStore } from "@/store/Institute/useProgramStore";

export default {
  name: "UpsertClassComp",
  components: {
    ValidationError,
    GenericSearchByFilter,
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
      ProgramStore: useProgramStore(),
      formData: {
        classID: "",
        name: "",
        notes: "",
        level: 1,
        programID: "",
      },
      programFilterTypes: [{ value: 1, label: "بحث باسم البرنامج" }],
      programResults: [],
      programName: "",
      programDataCard: null,
      isSearching: false,
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
    async onSearchProgram(payload) {
      this.isSearching = true;
      await this.ProgramStore.fetchPrograms();
      const q = String(payload.value || "").toLowerCase();
      this.programResults = (this.ProgramStore.programs || []).filter((p) =>
        String(p.name || "")
          .toLowerCase()
          .includes(q)
      );
      this.isSearching = false;
    },
    onProgramSelected(item) {
      this.FillProgramDataCard(item);
    },
    FillProgramDataCard(data) {
      this.programDataCard = data;
      if (this.programDataCard && this.programDataCard.programID) {
        this.formData.programID = this.programDataCard.programID;
        this.programName = this.programDataCard.name;
        this.showCard = true;
      } else {
        this.programDataCard = null;
        this.showCard = false;
      }
    },
    clearProgramSelection() {
      this.programDataCard = null;
      this.showCard = false;
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
        this.FillProgramDataCard(data);
      } else {
        this.programDataCard = null;
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
        if (data && data.programID != null) {
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
