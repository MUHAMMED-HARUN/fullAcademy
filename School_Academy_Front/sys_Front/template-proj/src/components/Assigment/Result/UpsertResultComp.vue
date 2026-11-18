<script setup>
import { computed, reactive, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  assignmentScopeInfos: { type: Array, default: () => [] },
  assignmentID: { type: String, default: "" },
  studentName: { type: String, default: "" },
  selectedResult: { type: Object, default: null },
  errors: { type: [String, Array, Object], default: null },
  saving: { type: Boolean, default: false },
});

const emit = defineEmits(["submit"]);
const formData = reactive({
  resultID: "",
  assignmentID: "",
  assignmentScopeInfoID: "",
  score: 0,
  maxScore: undefined,
  notes: "",
});

const isEditing = computed(() => Boolean(formData.resultID));
const formTitle = computed(() =>
  isEditing.value ? "تعديل نتيجة الاختبار" : "إضافة نتيجة الاختبار"
);
const selectedScopeInfo = computed(
  () =>
    props.assignmentScopeInfos.find(
      (item) => item.assignmentScopeInfoID === formData.assignmentScopeInfoID
    ) || null
);
const scoreExceedsMaximum = computed(
  () =>
    selectedScopeInfo.value &&
    Number(formData.score) > selectedScopeInfo.value.maxScore
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

function resetForm() {
  const result = props.selectedResult;
  formData.resultID = result?.resultID || "";
  formData.assignmentID = result?.assignmentID || props.assignmentID || "";
  formData.assignmentScopeInfoID = result?.assignmentScopeInfoID || "";
  formData.score = Number(result?.score ?? 0);
  formData.maxScore = result?.maxScore;
  formData.notes = result?.notes || "";
}

function onSubmit() {
  if (!selectedScopeInfo.value || scoreExceedsMaximum.value) return;
  emit("submit", {
    ...formData,
    maxScore: Number(selectedScopeInfo.value.maxScore),
  });
}

watch(
  () => [props.selectedResult, props.assignmentID, props.assignmentScopeInfos],
  resetForm,
  { immediate: true }
);
</script>

<template>
  <section class="space-y-4" dir="rtl">
    <h2 class="text-lg font-semibold">{{ formTitle }}</h2>
    <p v-if="errorText" class="alert-error" role="alert">{{ errorText }}</p>

    <form class="space-y-4" @submit.prevent="onSubmit">
      <div class="grid gap-3 sm:grid-cols-2">
        <div class="form-group">
          <label>الطالب</label>
          <input
            class="form-control"
            :value="studentName || selectedResult?.studentName || ''"
            readonly
          />
        </div>
        <div class="form-group">
          <label>الخطة الأكاديمية</label>
          <input
            class="form-control"
            :value="selectedResult?.academicPlanName || ''"
            readonly
          />
        </div>
      </div>

      <div class="form-group">
        <label for="result-scope-info">وحدة الاختبار</label>
        <select
          id="result-scope-info"
          v-model="formData.assignmentScopeInfoID"
          class="form-select"
          :disabled="saving || isEditing || !assignmentScopeInfos.length"
          required
        >
          <option value="" disabled>اختر وحدة اختبار</option>
          <option
            v-for="scopeInfo in assignmentScopeInfos"
            :key="scopeInfo.assignmentScopeInfoID"
            :value="scopeInfo.assignmentScopeInfoID"
          >
            {{ scopeInfo.stageInfoMatterNameName || scopeInfo.stageInfoMatter }}
          </option>
        </select>
        <p
          v-if="!assignmentScopeInfos.length"
          class="text-sm text-[var(--text-muted)]"
          role="status"
        >
          لا توجد وحدات متاحة لهذا الاختبار.
        </p>
        <ValidationError :messages="errors?.assignmentScopeInfoID" />
      </div>

      <div class="form-group">
        <label for="result-score">النتيجة</label>
        <input
          id="result-score"
          v-model.number="formData.score"
          class="form-control"
          type="number"
          min="0"
          :max="selectedScopeInfo?.maxScore"
          step="any"
          required
          :disabled="saving"
        />
        <p
          v-if="selectedScopeInfo"
          class="text-sm text-[var(--text-muted)]"
          role="status"
        >
          الحد الأعلى: {{ selectedScopeInfo.maxScore }} — الحد الأدنى للنجاح:
          {{ selectedScopeInfo.minPassScore }}
        </p>
        <ValidationError :messages="errors?.score" />
        <p
          v-if="scoreExceedsMaximum"
          class="text-sm text-[var(--color-danger)]"
          role="alert"
        >
          لا يمكن أن تتجاوز النتيجة {{ selectedScopeInfo.maxScore }}.
        </p>
      </div>

      <div class="form-group">
        <label for="result-notes">ملاحظات</label>
        <textarea
          id="result-notes"
          v-model="formData.notes"
          class="form-control"
          maxlength="500"
          :disabled="saving"
        />
        <ValidationError :messages="errors?.notes" />
      </div>

      <button
        class="btn btn-primary"
        type="submit"
        :disabled="saving || !selectedScopeInfo || scoreExceedsMaximum"
      >
        {{ saving ? "جارٍ الحفظ…" : "حفظ" }}
      </button>
    </form>
  </section>
</template>
