<script setup>
import { computed, reactive, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  stageOptions: { type: Array, default: () => [] },
  studentAcademicEnrollmentID: { type: String, default: "" },
  studentName: { type: String, default: "" },
  academicPlanName: { type: String, default: "" },
  studentStage: { type: Object, default: null },
  errors: { type: [Object, Array, String], default: () => ({}) },
  saving: { type: Boolean, default: false },
});

const emit = defineEmits(["submit"]);
const StageState = Object.freeze({
  Completed: 0,
  Processing: 1,
  Canceled: 2,
  Failed: 3,
});
const StageStateOptions = [
  { value: StageState.Processing, label: "قيد المعالجة" },
  { value: StageState.Completed, label: "مكتمل" },
  { value: StageState.Canceled, label: "تم الإلغاء" },
  { value: StageState.Failed, label: "فشل" },
];
const today = () => {
  const date = new Date();
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(
    2,
    "0"
  )}-${String(date.getDate()).padStart(2, "0")}`;
};

const form = reactive({
  studentStageID: "",
  studentAcademicEnrollmentID: "",
  stageID: "",
  studentStageState: StageState.Processing,
  studentStageDate: today(),
});
const isEditing = computed(() => !!form.studentStageID);
const formTitle = computed(() =>
  isEditing.value ? "تعديل متابعة المرحلة" : "إضافة متابعة مرحلة"
);

function onSubmit() {
  emit("submit", { ...form });
}

watch(
  () => [props.studentAcademicEnrollmentID, props.studentStage],
  ([enrollmentID, studentStage]) => {
    form.studentAcademicEnrollmentID =
      enrollmentID || studentStage?.studentAcademicEnrollmentID || "";
    form.studentStageID = studentStage?.studentStageID || "";
    form.stageID = studentStage?.stageID || "";
    form.studentStageState = Number(
      studentStage?.studentStageStateValue ?? StageState.Processing
    );
    form.studentStageDate = studentStage?.studentStageDate || today();
  },
  { immediate: true }
);
</script>

<template>
  <section dir="rtl" class="space-y-4">
    <h2 class="text-lg font-semibold">{{ formTitle }}</h2>
    <form class="space-y-4" @submit.prevent="onSubmit">
      <div class="grid gap-3 sm:grid-cols-2">
        <div class="form-group">
          <label>الطالب</label>
          <input class="form-control" :value="studentName" readonly />
        </div>
        <div class="form-group">
          <label>الخطة الأكاديمية</label>
          <input class="form-control" :value="academicPlanName" readonly />
        </div>
      </div>

      <div class="form-group">
        <label for="student-stage-stage">المرحلة ضمن الاشتراك</label>
        <select
          id="student-stage-stage"
          v-model="form.stageID"
          class="form-select"
          :disabled="isEditing || stageOptions.length === 0"
          required
        >
          <option value="" disabled>اختر مرحلة مشمولة في الخطة</option>
          <option
            v-for="stage in stageOptions"
            :key="stage.stageID"
            :value="stage.stageID"
          >
            {{ stage.stageName }}
          </option>
        </select>
        <p
          v-if="stageOptions.length === 0"
          class="mt-1 text-sm text-[var(--text-muted)]"
          role="status"
        >
          لا توجد مراحل مصرح بها في لقطة الاشتراك.
        </p>
        <ValidationError :messages="errors?.stageID" />
      </div>

      <div class="grid gap-3 sm:grid-cols-2">
        <div class="form-group">
          <label for="student-stage-state">الحالة</label>
          <select
            id="student-stage-state"
            v-model.number="form.studentStageState"
            class="form-select"
          >
            <option
              v-for="option in StageStateOptions"
              :key="option.value"
              :value="option.value"
            >
              {{ option.label }}
            </option>
          </select>
        </div>
        <div class="form-group">
          <label for="student-stage-date">التاريخ</label>
          <input
            id="student-stage-date"
            v-model="form.studentStageDate"
            class="form-control"
            type="date"
            required
          />
          <ValidationError :messages="errors?.studentStageDate" />
        </div>
      </div>

      <button
        type="submit"
        class="btn btn-primary"
        :disabled="
          saving ||
          !form.studentAcademicEnrollmentID ||
          !form.stageID ||
          !stageOptions.length
        "
      >
        {{ saving ? "جارٍ الحفظ…" : "حفظ" }}
      </button>
    </form>
  </section>
</template>
