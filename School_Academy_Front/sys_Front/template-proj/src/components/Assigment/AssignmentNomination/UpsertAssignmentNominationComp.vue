<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>الطالب :</label>
        <label>{{ studentName }}</label>
        <ValidationError :messages="errors?.date" />
      </div>
      <div class="matter-card-wrapper">
        <label>نطاق اختبار</label>
        <select
          v-model="formData.assignmentScopeID"
          required
          :disabled="saving || !assignmentScopesData.length"
        >
          <option value="" disabled>اختر نطاقاً مستحقاً</option>
          <option
            v-for="assigScope in assignmentScopesData"
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
        <input v-model="formData.date" type="date" required />
        <ValidationError :messages="errors?.date" />
      </div>

      <div class="form-group">
        <button
          type="submit"
          :disabled="
            saving ||
            !formData.assignmentScopeID ||
            !formData.studentAcademicEnrollmentID
          "
        >
          {{ saving ? "جارٍ الحفظ…" : "حفظ" }}
        </button>
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
    assignmentScopesData: {
      type: Array,
      default: () => [],
    },
    studentAcademicEnrollmentID: {
      type: String,
      default: "",
    },
    studentName: {
      type: String,
      default: "",
    },
    errors: {
      type: [String, Array, Object],
      default: null,
    },
    saving: { type: Boolean, default: false },
  },
  data() {
    return {
      AssignmentNomination: useAssignmentNominationStore(),
      formData: {
        assignmentNominationID: "",
        studentAcademicEnrollmentID: "",
        assignmentScopeID: "",
        date: new Date().toISOString().slice(0, 10),
      },
    };
  },
  emits: ["submit"],
  computed: {
    FormTitle() {
      return this.formData.assignmentNominationID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("submit", { ...this.formData });
    },

    fillSelectedAssignmentNomination() {
      if (this.AssignmentNomination.selectedAssignmentNomination) {
        const entity = this.AssignmentNomination.selectedAssignmentNomination;
        this.formData = {
          assignmentNominationID: entity.assignmentNominationID || "",
          assignmentScopeID: entity.assignmentScopeID || "",
          studentAcademicEnrollmentID: entity.studentAcademicEnrollmentID || "",
          date: entity.date || 0,
        };
      } else {
        this.formData.studentAcademicEnrollmentID =
          this.studentAcademicEnrollmentID;
      }
    },
  },

  mounted() {
    this.fillSelectedAssignmentNomination();
  },
};
</script>
