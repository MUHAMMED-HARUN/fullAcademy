<template>
  <div class="space-y-4" dir="rtl">
    <h2 class="text-lg font-semibold">{{ formTitle }}</h2>
    <p v-if="errorText" class="alert-error" role="alert">
      {{ errorText }}
    </p>

    <form class="space-y-4" @submit.prevent="onSubmit">
      <div class="grid gap-3 sm:grid-cols-2">
        <div class="form-group">
          <label>اسم الطالب</label>
          <input class="form-control" :value="studentName" readonly />
        </div>
        <div class="form-group">
          <label>الخطة الأكاديمية</label>
          <input class="form-control" :value="academicPlanName" readonly />
        </div>
      </div>

      <div class="form-group">
        <label for="daily-evaluation-stage-info">المادة / المرحلة</label>
        <select
          id="daily-evaluation-stage-info"
          v-model="formData.stageInfoID"
          class="form-select"
          :disabled="isEditing || studentStageInfoData.length === 0"
          required
          @change="onStageInfoChange"
        >
          <option value="" disabled>اختر تفاصيل المرحلة</option>
          <option
            v-for="stageInfo in studentStageInfoData"
            :key="stageInfo.stageInfoID"
            :value="stageInfo.stageInfoID"
          >
            {{ stageInfo.stageName }} — {{ stageInfo.matterName }}
          </option>
        </select>
        <p
          v-if="studentStageInfoData.length === 0"
          class="mt-1 text-sm text-[var(--text-muted)]"
          role="status"
        >
          لا توجد تفاصيل مراحل متاحة لهذا التسجيل.
        </p>
        <ValidationError :messages="errors?.stageInfoID" />
      </div>

      <div class="grid gap-3 sm:grid-cols-2">
        <div class="form-group">
          <label for="daily-evaluation-to">إلى</label>
          <input
            id="daily-evaluation-to"
            v-model.number="formData.to"
            class="form-control"
            type="number"
            min="0"
            :max="selectedStageInfo?.to"
            step="any"
            required
          />
          <ValidationError :messages="errors?.to" />
        </div>

        <div class="form-group">
          <label for="daily-evaluation-level">مستوى التقييم</label>
          <select
            id="daily-evaluation-level"
            v-model.number="formData.levelType"
            class="form-select"
            required
          >
            <option :value="enLevelType.VeryBad">سيء جداً</option>
            <option :value="enLevelType.Bad">سيء</option>
            <option :value="enLevelType.Good">جيد</option>
            <option :value="enLevelType.VeryGood">جيد جداً</option>
            <option :value="enLevelType.Excellent">ممتاز</option>
          </select>
          <ValidationError :messages="errors?.levelType" />
        </div>
      </div>

      <div class="form-group">
        <label for="daily-evaluation-date">التاريخ</label>
        <input
          id="daily-evaluation-date"
          v-model="formData.date"
          class="form-control"
          type="date"
          required
        />
        <ValidationError :messages="errors?.date" />
      </div>

      <p v-if="isFinished" class="alert-info" role="status">
        تم إنهاء نطاق هذه المادة.
      </p>
      <button
        class="btn btn-primary"
        type="submit"
        :disabled="
          saving ||
          isFinished ||
          !formData.stageInfoID ||
          studentStageInfoData.length === 0
        "
      >
        {{ saving ? "جارٍ الحفظ…" : "حفظ" }}
      </button>
    </form>
  </div>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";

const enLevelType = Object.freeze({
  VeryBad: 0,
  Bad: 1,
  Good: 2,
  VeryGood: 3,
  Excellent: 4,
});

const props = defineProps({
  studentStageInfoData: { type: Array, default: () => [] },
  lastDailyEvaluationData: { type: Object, default: null },
  selectedEvaluation: { type: Object, default: null },
  studentName: { type: String, default: "" },
  academicPlanName: { type: String, default: "" },
  studentAcademicEnrollmentID: { type: String, default: "" },
  errors: { type: [String, Array, Object], default: null },
  saving: { type: Boolean, default: false },
});

const emit = defineEmits(["submit", "stage-info-change"]);

const today = () => {
  const date = new Date();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${date.getFullYear()}-${month}-${day}`;
};

const dateForInput = (value) => {
  if (!value) return today();
  const date = new Date(value);
  return Number.isNaN(date.getTime())
    ? today()
    : date.toISOString().slice(0, 10);
};

const formData = reactive({
  dailyEvaluationID: "",
  to: 0,
  studentAcademicEnrollmentID: props.studentAcademicEnrollmentID,
  date: today(),
  levelType: enLevelType.VeryGood,
  stageInfoID: "",
});
const isFinished = ref(false);
const isEditing = computed(() => Boolean(formData.dailyEvaluationID));
const formTitle = computed(() =>
  isEditing.value ? "تعديل المتابعة اليومية" : "إضافة متابعة يومية"
);
const errorText = computed(() => {
  const flatten = (value) => {
    if (typeof value === "string") return [value];
    if (Array.isArray(value)) return value.flatMap(flatten);
    if (value && typeof value === "object")
      return Object.values(value).flatMap(flatten);
    return [];
  };
  return flatten(props.errors).join("، ");
});
const selectedStageInfo = computed(
  () =>
    props.studentStageInfoData.find(
      (item) => item.stageInfoID === formData.stageInfoID
    ) || null
);

function applyEvaluation(evaluation) {
  if (evaluation) {
    Object.assign(formData, {
      dailyEvaluationID: evaluation.dailyEvaluationID || "",
      to: Number(evaluation.to ?? 0),
      studentAcademicEnrollmentID:
        evaluation.studentAcademicEnrollmentID ||
        props.studentAcademicEnrollmentID,
      date: dateForInput(evaluation.date),
      levelType: Number(evaluation.levelType ?? enLevelType.VeryGood),
      stageInfoID: evaluation.stageInfoID || "",
    });
    return;
  }

  Object.assign(formData, {
    dailyEvaluationID: "",
    to: 0,
    studentAcademicEnrollmentID: props.studentAcademicEnrollmentID,
    date: today(),
    levelType: enLevelType.VeryGood,
    stageInfoID: props.studentStageInfoData[0]?.stageInfoID || "",
  });
  if (formData.stageInfoID) onStageInfoChange();
}

function onStageInfoChange() {
  const stageInfo = selectedStageInfo.value;
  isFinished.value = false;
  if (!stageInfo) {
    formData.to = 0;
    return;
  }

  formData.to = Number(stageInfo.from ?? 0);
  emit("stage-info-change", {
    studentAcademicEnrollmentID: formData.studentAcademicEnrollmentID,
    stageInfoID: formData.stageInfoID,
  });
}

function onSubmit() {
  emit("submit", { ...formData });
}

watch(
  () => props.lastDailyEvaluationData,
  (lastEvaluation) => {
    if (isEditing.value || !selectedStageInfo.value) return;
    if (
      lastEvaluation?.stageInfoID?.toString().toUpperCase() ===
      formData.stageInfoID?.toString().toUpperCase()
    ) {
      if (lastEvaluation.levelType > 1) {
        formData.to = Number(lastEvaluation.to) + 1;
      } else formData.to = Number(lastEvaluation.from);

      isFinished.value = formData.to > Number(selectedStageInfo.value.to);
    } else {
      formData.to = Number(selectedStageInfo.value.from ?? 0);
      isFinished.value = false;
    }
  }
);

watch(
  () => props.selectedEvaluation,
  (evaluation) => applyEvaluation(evaluation),
  { immediate: true }
);

watch(
  () => props.studentStageInfoData,
  (items) => {
    if (!isEditing.value && !formData.stageInfoID && items.length > 0)
      applyEvaluation(null);
  }
);
</script>
