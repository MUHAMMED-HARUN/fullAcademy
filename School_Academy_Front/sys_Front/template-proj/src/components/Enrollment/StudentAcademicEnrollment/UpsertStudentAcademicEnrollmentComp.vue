<script setup>
import { computed, reactive, watch } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  errors: { type: [Object, Array, String], default: () => ({}) },
  studentResults: { type: Array, default: () => [] },
  studentLoading: { type: Boolean, default: false },
  studentError: { type: [String, Array, Object], default: null },
  planResults: { type: Array, default: () => [] },
  planLoading: { type: Boolean, default: false },
  planError: { type: [String, Array, Object], default: null },
  enrollment: { type: Object, default: null },
  saving: { type: Boolean, default: false },
});

const emit = defineEmits([
  "submit",
  "student-search",
  "plan-search",
  "student-clear",
  "plan-clear",
]);

const today = () => new Date().toISOString().slice(0, 10);
const form = reactive({
  studentID: "",
  studentName: "",
  academicPlanID: "",
  academicPlanName: "",
  enrollmentDate: today(),
  validFrom: today(),
  validTo: "",
});

const studentFilters = [
  { value: "FirstName", label: "الاسم الأول" },
  { value: "NationalNumber", label: "الرقم الوطني" },
];
const planFilters = [{ value: "Name", label: "اسم الخطة" }];
const readOnly = computed(() => !!props.enrollment);

function selectStudent(item) {
  form.studentID = item.studentID;
  form.studentName = item.displayName;
}

function selectPlan(item) {
  form.academicPlanID = item.academicPlanID;
  form.academicPlanName = item.displayName;
  const planStart = item.effectiveFrom || today();
  form.validFrom = planStart > today() ? planStart : today();
  form.enrollmentDate = form.validFrom;
  form.validTo = item.effectiveTo || "";
}

function clearStudent() {
  form.studentID = "";
  form.studentName = "";
  emit("student-clear");
}

function clearPlan() {
  form.academicPlanID = "";
  form.academicPlanName = "";
  form.validTo = "";
  emit("plan-clear");
}

function submit() {
  emit("submit", {
    studentID: form.studentID,
    academicPlanID: form.academicPlanID,
    enrollmentDate: form.enrollmentDate,
    validFrom: form.validFrom,
    validTo: form.validTo,
  });
}

watch(
  () => props.enrollment,
  (value) => {
    Object.assign(form, {
      studentID: value?.studentID || "",
      studentName: value?.studentName || "",
      academicPlanID: value?.academicPlanID || "",
      academicPlanName: value?.academicPlanName || "",
      enrollmentDate: value?.enrollmentDate || today(),
      validFrom: value?.validFrom || today(),
      validTo: value?.validTo || "",
    });
  },
  { immediate: true }
);
</script>

<template>
  <section dir="rtl" class="space-y-4">
    <h2 class="text-lg font-semibold">
      {{
        readOnly ? "تفاصيل الاشتراك الأكاديمي" : "تسجيل الطالب في خطة أكاديمية"
      }}
    </h2>
    <form class="space-y-4" @submit.prevent="submit">
      <div class="form-group">
        <label>الطالب</label>
        <GenericSearchByFilter
          :model-value="form.studentID"
          :name-value="form.studentName"
          :results="studentResults"
          :filter-types="studentFilters"
          id-field="studentID"
          name-field="displayName"
          placeholder="ابحث بالاسم أو الرقم الوطني"
          :loading="studentLoading"
          :error="studentError"
          :disabled="readOnly"
          @select="selectStudent"
          @update:name-value="form.studentName = $event"
          @clear="clearStudent"
          @search="$emit('student-search', $event)"
        />
        <ValidationError :messages="errors?.studentID" />
      </div>

      <div class="form-group">
        <label>الخطة الأكاديمية الفعالة</label>
        <GenericSearchByFilter
          :model-value="form.academicPlanID"
          :name-value="form.academicPlanName"
          :results="planResults"
          :filter-types="planFilters"
          id-field="academicPlanID"
          name-field="displayName"
          placeholder="ابحث عن خطة فعالة"
          :loading="planLoading"
          :error="planError"
          :disabled="readOnly"
          @select="selectPlan"
          @update:name-value="form.academicPlanName = $event"
          @clear="clearPlan"
          @search="$emit('plan-search', $event)"
        />
        <ValidationError :messages="errors?.academicPlanID" />
      </div>

      <div
        v-if="readOnly"
        class="rounded-lg border border-[var(--border-color)] p-3"
      >
        <h3 class="mb-2 font-semibold">لقطة StageInfo المعتمدة وقت التسجيل</h3>
        <ul class="list-inside list-disc space-y-1 text-sm">
          <li v-for="item in enrollment?.items || []" :key="item.stageInfoID">
            {{ item.stageName }} — {{ item.matterName }} ({{ item.from }}–{{
              item.to
            }})
          </li>
        </ul>
      </div>

      <div class="grid gap-4 sm:grid-cols-3">
        <div class="form-group">
          <label>تاريخ التسجيل</label>
          <input
            v-model="form.enrollmentDate"
            type="date"
            :disabled="readOnly"
          />
          <ValidationError :messages="errors?.enrollmentDate" />
        </div>
        <div class="form-group">
          <label>الصلاحية من</label>
          <input v-model="form.validFrom" type="date" :disabled="readOnly" />
          <ValidationError :messages="errors?.validFrom" />
        </div>
        <div class="form-group">
          <label>الصلاحية إلى (اختياري)</label>
          <input v-model="form.validTo" type="date" :disabled="readOnly" />
          <ValidationError :messages="errors?.validTo" />
        </div>
      </div>

      <button
        v-if="!readOnly"
        type="submit"
        class="btn btn-primary"
        :disabled="saving"
      >
        {{ saving ? "جارٍ الحفظ" : "حفظ الاشتراك" }}
      </button>
    </form>
  </section>
</template>
