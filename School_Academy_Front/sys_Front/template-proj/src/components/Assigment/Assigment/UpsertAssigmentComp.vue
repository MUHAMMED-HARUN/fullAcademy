<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>الطالب:</label>
        <label>{{
          nominationInfo?.studentName ||
          AssignmentStore.selectedassignments?.studentName
        }}</label>
      </div>
      <div class="form-group">
        <label>نطاق الاختبار:</label>
        <label>{{
          nominationInfo?.assignmentScopeName ||
          AssignmentStore.selectedassignments?.assigmentScopeName
        }}</label>
      </div>

      <div class="form-group">
        <label for="assignment-date">موعد الاختبار</label>
        <input
          id="assignment-date"
          v-model="formData.scheduledDate"
          type="date"
          required
        />
        <ValidationError :messages="errors?.scheduledDate" />
      </div>

      <div class="form-group">
        <button
          type="submit"
          :disabled="
            saving ||
            !formData.assignmentNominationID ||
            !formData.scheduledDate
          "
        >
          {{ saving ? "جارٍ الحفظ…" : "حفظ" }}
        </button>
      </div>
    </form>
  </div>
</template>

<script>
import { useassignmentstore } from "@/store/Assignment/AssignmentStore";
import ValidationError from "@/components/general/ValidationError.vue";

const getLocalDate = () => {
  const date = new Date();
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(
    2,
    "0"
  )}-${String(date.getDate()).padStart(2, "0")}`;
};

export default {
  name: "UpsertAssigmentComp",
  components: { ValidationError },

  props: {
    nominationInfo: {
      type: Object,
      default: () => ({}),
    },
    errors: { type: [String, Array, Object], default: null },
    saving: { type: Boolean, default: false },
  },
  data() {
    return {
      AssignmentStore: useassignmentstore(),
      formData: {
        assignmentID: "",
        assignmentNominationID: "",
        scheduledDate: getLocalDate(),
      },
    };
  },
  emits: ["submit"],
  computed: {
    FormTitle() {
      return this.formData.assignmentID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    localDate() {
      return getLocalDate();
    },
    onSubmit() {
      this.$emit("submit", { ...this.formData });
    },
    fillAssigment() {
      if (this.AssignmentStore.selectedassignments) {
        const entity = this.AssignmentStore.selectedassignments;
        this.formData = {
          assignmentID: entity.assignmentID || "",
          assignmentNominationID: entity.assignmentNominationID || "",
          scheduledDate: entity.scheduledDate || this.localDate(),
        };
      } else {
        this.formData.assignmentNominationID =
          this.nominationInfo?.assignmentNominationID || "";
      }
    },
  },

  mounted() {
    this.fillAssigment();
  },
};
</script>
