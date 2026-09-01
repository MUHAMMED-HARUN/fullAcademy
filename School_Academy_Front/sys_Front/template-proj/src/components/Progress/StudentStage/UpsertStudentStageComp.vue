<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>الطالب :</label>
        <label>{{ studentEnrollmentName }}</label>
        <ValidationError :messages="errors?.date" />
      </div>
      <div class="matter-card-wrapper">
        <label>المرحلة</label>
        <select v-model="formData.stageID">
          <option disabled selected>اختر</option>
          <option
            v-for="stage in stages"
            :key="stage.stageID"
            :value="stage.stageID"
          >
            {{ stage.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.stageID" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useStudentStageStore } from "@/store/Progress/StudentStageStore";

export default {
  name: "UpsertStudentStageComp",
  components: {
    ValidationError,
  },
  props: {
    stages: {
      type: Object,
      default: () => ({}),
    },
    studentEnrollmentID: {
      type: String,
      default: () => "",
    },
    studentEnrollmentName: {
      type: String,
      default: () => "",
    },
    errors: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      StudentStageStore: useStudentStageStore(),
      formData: {
        studentStageID: null,
        stageID: null,
        enrollmentStudentID: null,
      },
    };
  },
  emits: ["onSubmit"],
  computed: {
    FormTitle() {
      return this.formData.studentStageID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },

    fillSelectedAssignmentNomination() {
      if (this.StudentStageStore.selectedStudentStage) {
        const entity = this.StudentStageStore.selectedStudentStage;
        this.formData = {
          studentStageID: entity.studentStageID || "",
          stageID: entity.stageID || "",
          enrollmentStudentID: entity.enrollmentStudentID || "",
        };
      } else if (
        this.studentEnrollmentID !== null ||
        this.studentEnrollmentID !== ""
      ) {
        this.formData.enrollmentStudentID = this.studentEnrollmentID;
      }
    },
  },

  mounted() {
    this.fillSelectedAssignmentNomination();
  },
};
</script>
