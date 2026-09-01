<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>الطالب:</label>
        <label>{{ NominationInfo?.studentName }}</label>
      </div>
      <div class="form-group">
        <label>نطاق الاختبار:</label>
        <label>{{ NominationInfo?.assignmentScopeName }}</label>
      </div>

      <!-- تضمين مكون العنوان الفرعي بربط v-model -->

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import { useassignmentstore } from "@/store/Assignment/AssignmentStore";

export default {
  name: "UpsertAssigmentComp",

  props: {
    NominationInfo: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      assignmentstore: useassignmentstore(),
      formData: {
        assignmentID: "",
        assignmentNominationID: "",
      },
    };
  },
  emits: ["onSubmit"],
  computed: {
    FormTitle() {
      return this.formData.assignmentID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    fillAssigment() {
      if (this.assignmentstore.selectedassignments) {
        const entity = this.assignmentstore.selectedassignments;
        this.formData = {
          assignmentID: entity.assignmentID || "",
          assignmentNominationID: entity.assignmentNominationID || "",
        };
      } else {
        this.formData.assignmentNominationID =
          this.NominationInfo?.assignmentNominationID;
      }
    },
  },

  mounted() {
    this.fillAssigment();
  },
};
</script>
