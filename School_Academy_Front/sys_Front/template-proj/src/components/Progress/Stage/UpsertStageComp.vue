<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المرحلة:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-filter mt-4">
        <label>البحث عن مشروع:</label>
        <select v-model="programSearchType">
          <option :value="enProgramSearchType.ProgramName">
            بحث باسم المشروع
          </option>
        </select>
        <input v-model="programSearchValue" type="text" />
        <button @click.prevent.stop="onSearchProgram">بحث عن مشروع</button>
      </div>

      <div class="program-card-wrapper">
        <ProgramCard v-if="showProgramCard" :-data-req="programData" />
        <ValidationError :messages="errors?.programID" />
      </div>

      <div class="form-group">
        <label>المرحلة السابقة:</label>
        <select v-model="formData.prevStageID">
          <option value="" disabled>اختر نوع المادة</option>
          <option
            v-for="stage in Stages"
            :value="stage.StageID"
            :key="stage.stageID"
          >
            {{ stage.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.prevStageID" />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.note" type="text" />
        <ValidationError :messages="errors?.note" />
      </div>

      <!-- تضمين مكون العنوان الفرعي بربط v-model -->

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import ProgramCard from "@/components/Institute/Program/ProgramCard.vue";
import { useStageStore } from "@/store/Progress/StageStore";
const enProgramSearchType = Object.freeze({ ID: 0, ProgramName: 1 });

export default {
  name: "UpsertStageComp",
  components: {
    ValidationError,
    ProgramCard,
  },
  props: {
    Stages: {
      type: Object,
      default: () => ({}),
    },
    programData: {
      type: Object,
      default: () => ({}),
    },
    errors: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      enProgramSearchType: enProgramSearchType,
      programSearchType: enProgramSearchType.ProgramName,
      programSearchValue: "",
      showProgramCard: false,

      StageStore: useStageStore(),
      formData: {
        stageID: "",
        name: "",
        note: "",
        programID: "",
        prevStageID: "",
      },
    };
  },
  emits: ["onSubmit", "onSearchProgram"],
  computed: {
    FormTitle() {
      return this.formData.stageID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onSearchProgram() {
      this.$emit(
        "onSearchProgram",
        this.programSearchType,
        this.programSearchValue
      );
    },
    fillSelectedStage() {
      if (this.StageStore.selectedStage) {
        const entity = this.StageStore.selectedStage;
        this.formData = {
          stageID: entity.stageID || "",
          name: entity.name || "",
          note: entity.note || "",
          programID: entity.programID || "",
          prevStageID: entity.prevStageID || "",
        };
      }
    },
    handleProgram(data, isFound) {
      if (isFound) {
        this.formData.programID = data.programID;
        this.showProgramCard = true;
      } else {
        this.showProgramCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedStage();
  },
  watch: {
    programData: {
      handler(data) {
        if (data && data.programID != null) {
          this.handleProgram(data, true);
        } else {
          this.handleProgram(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
