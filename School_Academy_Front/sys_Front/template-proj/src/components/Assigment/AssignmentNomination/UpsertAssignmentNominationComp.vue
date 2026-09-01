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
        <label>نطاق اختبار</label>
        <select v-model="formData.assignmentScopeID">
          <option disabled selected>اختر نطاق</option>
          <option
            v-for="assigScope in AssigmentScopesData"
            :key="assigScope.assignmentScopeID"
            :value="assigScope.assignmentScopeID"
          >
            {{ assigScope.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.assignmentScopeID" />
      </div>

      <div class="form-group">
        <label>تاريخ :</label>
        <input v-model="formData.date" type="date" />
        <ValidationError :messages="errors?.date" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useAssignmentNominationStore } from "@/store/Assignment/AssignmentNominationStore";

export default {
  name: "UpsertStageInfoComp",
  components: {
    ValidationError,
  },
  props: {
    AssigmentScopesData: {
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
      AssignmentNomination: useAssignmentNominationStore(),
      formData: {
        assignmentNominationID: "",
        studentEnrollmentID: null,
        assignmentScopeID: null,
        date: "",
      },
    };
  },
  emits: ["onSubmit"],
  computed: {
    FormTitle() {
      return this.formData.assignmentNominationID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },

    fillSelectedAssignmentNomination() {
      if (this.AssignmentNomination.selectedAssignmentNomination) {
        const entity = this.AssignmentNomination.selectedAssignmentNomination;
        this.formData = {
          assignmentNominationID: entity.assignmentNominationID || "",
          assignmentScopeID: entity.assignmentScopeID || "",
          studentEnrollmentID: entity.studentEnrollmentID || "",
          date: entity.date || 0,
        };
      } else if (
        this.studentEnrollmentID !== null ||
        this.studentEnrollmentID !== ""
      ) {
        alert(this.studentEnrollmentID);
        this.formData.studentEnrollmentID = this.studentEnrollmentID;
      }
    },
  },

  mounted() {
    this.fillSelectedAssignmentNomination();
  },
};
</script>
