<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم الخطة:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-group">
        <label>نوع الرسوم:</label>
        <select v-model.number="formData.feeType">
          <option disabled value="">اختر النوع</option>
          <option
            v-for="opt in feeTypeOptions"
            :key="opt.value"
            :value="opt.value"
          >
            {{ opt.label }}
          </option>
        </select>
        <ValidationError :messages="errors?.feeType" />
      </div>

      <div class="form-group">
        <label>المبلغ:</label>
        <input
          v-model.number="formData.amount"
          type="number"
          min="0"
          step="0.01"
        />
        <ValidationError :messages="errors?.amount" />
      </div>

      <div class="form-filter mt-4">
        <label>البحث عن برنامج:</label>
        <select v-model="programSearchType">
          <option :value="enProgramSearchType.Name">بحث باسم البرنامج</option>
        </select>
        <input v-model="programSearchValue" type="text" />
        <button @click.prevent.stop="onSearchProgram">بحث عن برنامج</button>
      </div>
      <div class="class-card-wrapper">
        <ProgramCard v-if="showProgramCard" :-data-req="programData" />
      </div>

      <div class="form-filter mt-4">
        <label>البحث عن صف:</label>
        <select v-model="classSearchType">
          <option :value="enClassSearchType.Name">بحث باسم الصف</option>
        </select>
        <input v-model="classSearchValue" type="text" />
        <button @click.prevent.stop="onSearchClass">بحث عن صف</button>
      </div>
      <div class="class-card-wrapper">
        <ClassCard v-if="showClassCard" :-data-req="classData" />
      </div>

      <div class="form-filter mt-4">
        <label>البحث عن مرحلة:</label>
        <select v-model="stageSearchType">
          <option :value="enStageSearchType.Name">بحث باسم المرحلة</option>
        </select>
        <input v-model="stageSearchValue" type="text" />
        <button @click.prevent.stop="onSearchStage">بحث عن مرحلة</button>
      </div>
      <div class="class-card-wrapper">
        <StageCard v-if="showStageCard" :-data-req="stageData" />
      </div>

      <div class="form-group">
        <label>تفصيل المرحلة:</label>
        <select v-model="formData.stageInfoID">
          <option value="">بدون</option>
          <option
            v-for="info in filteredStageInfos"
            :key="info.stageInfoID"
            :value="info.stageInfoID"
          >
            {{ info.stageName }} - {{ info.matterName }} ({{ info.from }} -
            {{ info.to }})
          </option>
        </select>
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import ProgramCard from "@/components/Institute/Program/ProgramCard.vue";
import ClassCard from "@/components/Institute/Class/ClassCard.vue";
import StageCard from "@/components/Progress/Stage/StageCard.vue";
import { useFeeStructureStore } from "@/store/Finance/FeeStructureStore";
import { FeeType, FeeTypeOptions } from "@/Generic/FinanceEnums";

const enProgramSearchType = Object.freeze({ ID: 0, Name: 1 });
const enClassSearchType = Object.freeze({ ID: 0, Name: 1 });
const enStageSearchType = Object.freeze({ ID: 0, Name: 1 });

export default {
  name: "UpsertFeeStructureComp",
  components: {
    ValidationError,
    ProgramCard,
    ClassCard,
    StageCard,
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
    classData: {
      type: Object,
      default: () => ({}),
    },
    stageData: {
      type: Object,
      default: () => ({}),
    },
    stageInfos: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      FeeStructureStore: useFeeStructureStore(),
      feeTypeOptions: FeeTypeOptions,
      enProgramSearchType,
      enClassSearchType,
      enStageSearchType,
      programSearchType: enProgramSearchType.Name,
      classSearchType: enClassSearchType.Name,
      stageSearchType: enStageSearchType.Name,
      programSearchValue: "",
      classSearchValue: "",
      stageSearchValue: "",
      showProgramCard: false,
      showClassCard: false,
      showStageCard: false,
      formData: {
        feeStructureID: "",
        name: "",
        feeType: FeeType.Tuition,
        amount: 0,
        programID: "",
        classID: "",
        stageID: "",
        stageInfoID: "",
        notes: "",
      },
    };
  },
  emits: ["onSubmit", "onSearchProgram", "onSearchClass", "onSearchStage"],
  computed: {
    FormTitle() {
      return this.formData.feeStructureID
        ? "تعديل خطة الرسوم"
        : "إضافة خطة رسوم";
    },
    filteredStageInfos() {
      if (!this.formData.stageID) return this.stageInfos || [];
      return (this.stageInfos || []).filter(
        (info) => info.stageID === this.formData.stageID
      );
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
    onSearchClass() {
      this.$emit("onSearchClass", this.classSearchType, this.classSearchValue);
    },
    onSearchStage() {
      this.$emit("onSearchStage", this.stageSearchType, this.stageSearchValue);
    },
    fillSelected() {
      const entity = this.FeeStructureStore.selectedFeeStructure;
      if (!entity) return;
      this.formData = {
        feeStructureID: entity.feeStructureID || "",
        name: entity.name || "",
        feeType: entity.feeType || FeeType.Tuition,
        amount: entity.amount || 0,
        programID: entity.programID || "",
        classID: entity.classID || "",
        stageID: entity.stageID || "",
        stageInfoID: entity.stageInfoID || "",
        notes: entity.notes || "",
      };
    },
  },
  mounted() {
    this.fillSelected();
  },
  watch: {
    programData: {
      handler(data) {
        if (data && data.programID) {
          this.formData.programID = data.programID;
          this.showProgramCard = true;
        } else {
          this.showProgramCard = false;
        }
      },
      immediate: true,
      deep: true,
    },
    classData: {
      handler(data) {
        if (data && data.classID) {
          this.formData.classID = data.classID;
          this.showClassCard = true;
        } else {
          this.showClassCard = false;
        }
      },
      immediate: true,
      deep: true,
    },
    stageData: {
      handler(data) {
        if (data && data.stageID) {
          this.formData.stageID = data.stageID;
          this.showStageCard = true;
        } else {
          this.showStageCard = false;
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
