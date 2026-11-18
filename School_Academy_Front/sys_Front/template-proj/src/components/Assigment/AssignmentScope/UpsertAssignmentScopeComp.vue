<script setup>
import { computed, reactive, ref, watch } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  initialScope: { type: Object, default: null },
  assignmentScopes: { type: Array, default: () => [] },
  stageResults: { type: Array, default: () => [] },
  stageLoading: { type: Boolean, default: false },
  stageError: { type: [String, Array, Object], default: null },
  errors: { type: [Object, Array, String], default: () => ({}) },
  saving: { type: Boolean, default: false },
});

const emit = defineEmits(["submit", "search-stage", "clear-stage"]);

const form = reactive({
  assignmentScopeID: "",
  name: "",
  notes: "",
  stageID: "",
  prevAssignmentScopeID: "",
});
const selectedStageName = ref("");
const selectedStage = ref(null);
const isEditing = computed(() => !!form.assignmentScopeID);
const formTitle = computed(() =>
  isEditing.value ? "تعديل نطاق الاختبار" : "إضافة نطاق اختبار"
);

watch(
  () => props.initialScope,
  (scope) => {
    form.assignmentScopeID = scope?.assignmentScopeID || "";
    form.name = scope?.name || "";
    form.notes = scope?.notes || "";
    form.stageID = scope?.stageID || "";
    form.prevAssignmentScopeID = scope?.prevAssignmentScopeID || "";
    selectedStageName.value = scope?.stageName || "";
    selectedStage.value = scope?.stageID
      ? { stageID: scope.stageID, name: scope.stageName || "" }
      : null;
  },
  { immediate: true }
);

function onStageSelected(stage) {
  selectedStage.value = stage || null;
  selectedStageName.value = stage?.name || "";
  form.stageID = stage?.stageID || "";
}

function onClearStage() {
  selectedStage.value = null;
  selectedStageName.value = "";
  form.stageID = "";
  emit("clear-stage");
}

function submit() {
  emit("submit", { ...form });
}
</script>

<template>
  <section dir="rtl" class="space-y-4">
    <h2 class="text-lg font-semibold">{{ formTitle }}</h2>
    <form class="space-y-4" @submit.prevent="submit">
      <div class="form-group">
        <label for="assignment-scope-name">اسم نطاق الاختبار</label>
        <input
          id="assignment-scope-name"
          v-model.trim="form.name"
          class="form-control"
          type="text"
          required
        />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-group">
        <label>المرحلة</label>
        <GenericSearchByFilter
          v-model="form.stageID"
          v-model:name-value="selectedStageName"
          :filter-types="[{ value: 1, label: 'اسم المرحلة' }]"
          :results="stageResults"
          :loading="stageLoading"
          :error="stageError"
          id-field="stageID"
          name-field="name"
          placeholder="ابحث عن مرحلة"
          empty-message="لا توجد مراحل مطابقة"
          @search="$emit('search-stage', $event.value)"
          @select="onStageSelected"
          @clear="onClearStage"
        />
        <div
          v-if="selectedStage"
          class="mt-2 rounded-lg border border-[var(--border-color)] bg-[var(--bg-body)] p-3 text-sm"
        >
          {{ selectedStage.name }}
        </div>
        <ValidationError :messages="errors?.stageID" />
      </div>

      <div class="form-group">
        <label for="assignment-scope-prev">ربط الاختبار السابق</label>
        <select
          id="assignment-scope-prev"
          v-model="form.prevAssignmentScopeID"
          class="form-select"
        >
          <option value="" disabled>اختر الاختبار السابق</option>
          <option
            v-for="scope in assignmentScopes"
            :key="scope.assignmentScopeID"
            :value="scope.assignmentScopeID"
          >
            {{ scope.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.prevAssignmentScopeID" />
      </div>

      <div class="form-group">
        <label for="assignment-scope-notes">ملاحظات</label>
        <input
          id="assignment-scope-notes"
          v-model.trim="form.notes"
          class="form-control"
          type="text"
        />
        <ValidationError :messages="errors?.notes" />
      </div>

      <button
        type="submit"
        class="btn btn-primary"
        :disabled="saving || !form.stageID || !form.name"
      >
        {{ saving ? "جارٍ الحفظ…" : "حفظ" }}
      </button>
    </form>
  </section>
</template>
