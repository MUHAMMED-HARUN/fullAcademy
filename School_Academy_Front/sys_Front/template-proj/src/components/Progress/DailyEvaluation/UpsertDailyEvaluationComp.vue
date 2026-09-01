<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم الطالب :</label>
        <label> {{ studentName }}</label>
      </div>

      <div class="form-group">
        <label>الشعبة :</label>
        <label> {{ groupName }}</label>
      </div>

      <div class="form-filter mt-4">
        <label> المادة :</label>
        <select v-on:change="onStageInfoChange" v-model="formData.stageInfoID">
          <option
            v-for="stageinfo in StudentStageInfoData"
            :key="stageinfo.stageInfoID"
            :value="stageinfo.stageInfoID"
          >
            {{ stageinfo.matterName }}
          </option>
        </select>
        <ValidationError :messages="errors?.stageInfoID" />
      </div>

      <div class="form-group">
        <label>الى:</label>
        <input v-model="formData.to" type="number" />
        <ValidationError :messages="errors?.to" />
      </div>

      <div class="form-group">
        <label>تقييم:</label>
        <select v-model="formData.levelType">
          <option :value="enLevelType.VeryBad">سيئ جدا</option>
          <option :value="enLevelType.Bad">سيئ</option>
          <option :value="enLevelType.Good">جيد</option>
          <option :value="enLevelType.VeryGood">جيد جدا</option>
          <option :value="enLevelType.Excilent">ممتاز</option>
        </select>
        <ValidationError :messages="errors?.levelType" />
      </div>

      <div class="form-group">
        <label>التاريخ:</label>
        <input v-model="formData.date" type="date" />
        <ValidationError :messages="errors?.date" />
      </div>

      <div v-if="isFinished" class="form-group">
        <label>تم انهاء هذه الفئة</label>
      </div>
      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useDailyEvaluationStore } from "@/store/Progress/DailyEvaluationStore.js";
const enLevelType = Object.freeze({
  VeryBad: 0,
  Bad: 1,
  Good: 2,
  VeryGood: 3,
  Excilent: 4,
});

export default {
  name: "UpsertDailyEvaluationComp",
  components: {
    ValidationError,
  },
  props: {
    StudentStageInfoData: {
      type: Object,
      default: () => ({}),
    },
    LastDailyEvaluationData: {
      type: Object,
      default: () => ({}),
    },
    studentName: {
      type: Object,
      default: () => ({}),
    },
    groupName: {
      type: Object,
      default: () => ({}),
    },
    enrollmentID: {
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
      enLevelType: enLevelType,
      DailyEvaluationStore: useDailyEvaluationStore(),
      isFinished: false,
      selectedStage: null,
      formData: {
        dailyEvaluationID: "",
        to: "",
        studentEnrollmentID: "",
        date: new Date().toISOString().substring(0, 10),
        levelType: 3,
        stageInfoID: "",
      },
    };
  },
  emits: ["onSubmit", "onStageInfoChange"],
  computed: {
    FormTitle() {
      return this.formData.dailyEvaluationID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onStageInfoChange() {
      if (Array.isArray(this.StudentStageInfoData)) {
        this.selectedStage =
          this.StudentStageInfoData.find(
            (stage) => stage.stageInfoID == this.formData.stageInfoID
          ) || null;
        this.formData.to = this.selectedStage?.from;
      } else {
        this.selectedStage = null;
      }
      this.$emit("onStageInfoChange", this.formData);
    },

    fillSelectedDailyEvaluation() {
      const entity = this.DailyEvaluationStore.selectedEvaluation;
      if (entity) {
        this.formData = {
          dailyEvaluationID: entity?.dailyEvaluationID || "",
          to: entity?.to + 1 || "",
          studentEnrollmentID: entity.enrollmentID || "",
          stageInfoID: entity.stageInfoID || "",
          date: entity.date || "",
          levelType: entity.levelType,
        };
      } else {
        this.formData.to = this.StudentStageInfoData[0]?.from;
        this.formData.stageInfoID = this.StudentStageInfoData[0]?.stageInfoID;
      }
      if (this.enrollmentID !== "")
        this.formData.studentEnrollmentID = this.enrollmentID;
      this.onStageInfoChange();
    },
    handleDailyEvaluation(data, isFound) {
      if (isFound) {
        if (this.selectedStage && this.LastDailyEvaluationData !== null) {
          if (this.selectedStage?.to <= this.LastDailyEvaluationData?.to) {
            this.isFinished = true;
          }
          if (this.LastDailyEvaluationData !== null) {
            this.formData.to = this.LastDailyEvaluationData?.to + 1;
          }
        } else {
          if (this.LastDailyEvaluationData !== null) {
            this.formData.to = this.LastDailyEvaluationData?.to + 1;
          }
        }
      }
    },
  },

  mounted() {
    this.fillSelectedDailyEvaluation();
  },
  watch: {
    LastDailyEvaluationData: {
      handler(data) {
        if (data && data?.dailyEvaluationID != null) {
          this.handleDailyEvaluation(data, true);
        } else {
          this.handleDailyEvaluation(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
