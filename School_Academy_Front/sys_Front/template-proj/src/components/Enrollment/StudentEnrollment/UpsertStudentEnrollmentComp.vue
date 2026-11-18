<script setup>
import { computed, onMounted, reactive, watch } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  errors: { type: [Object, Array, String], default: () => ({}) },
  groupData: { type: Object, default: null },
  enrollmentResults: { type: Array, default: () => [] },
  enrollmentLoading: { type: Boolean, default: false },
  enrollmentError: { type: [String, Array, Object], default: null },
});

const emit = defineEmits([
  "onSubmit",
  "onSearchAcademicEnrollment",
  "onClearAcademicEnrollment",
]);

const formData = reactive({
  studentAcademicEnrollmentID: "",
  groupID: "",
  groupEnrollmentID: "",
  date: new Date().toISOString().slice(0, 10),
});
const selectedAcademicEnrollment = reactive({ item: null });
const searchTypes = [
  { value: "student", label: "اسم الطالب" },
  { value: "plan", label: "اسم الخطة" },
];
const groupName = computed(() => props.groupData?.groupName || "");

function selectEnrollment(item) {
  selectedAcademicEnrollment.item = item;
  formData.studentAcademicEnrollmentID = item.studentAcademicEnrollmentID;
}

function clearEnrollment() {
  selectedAcademicEnrollment.item = null;
  formData.studentAcademicEnrollmentID = "";
  emit("onClearAcademicEnrollment");
}

function submit() {
  emit("onSubmit", {
    groupEnrollmentID: formData.groupEnrollmentID,
    studentAcademicEnrollmentID: formData.studentAcademicEnrollmentID,
    groupID:
      formData.groupID ||
      props.groupData?.groupID ||
      props.groupData?.GroupID ||
      props.groupData?.groupId ||
      "",
    date: formData.date,
  });
}

watch(
  () => props.groupData,
  (group) => {
    formData.groupID = group?.groupID || group?.GroupID || group?.groupId || "";
    clearEnrollment();
  },
  { immediate: true, deep: true }
);

onMounted(() => {
  if (!formData.groupID && props.groupData) {
    formData.groupID =
      props.groupData.groupID ||
      props.groupData.GroupID ||
      props.groupData.groupId ||
      "";
  }
});
</script>

<template>
  <section dir="rtl" class="space-y-4">
    <h2 class="text-lg font-semibold">إضافة اشتراك قائم إلى مجموعة</h2>
    <div class="form-group">
      <label>المجموعة</label>
      <input :value="groupName" type="text" disabled />
    </div>
    <form class="space-y-4" @submit.prevent="submit">
      <div class="form-group">
        <label>الاشتراك الأكاديمي المؤهل</label>
        <GenericSearchByFilter
          v-model="formData.studentAcademicEnrollmentID"
          :results="enrollmentResults"
          :filter-types="searchTypes"
          id-field="studentAcademicEnrollmentID"
          name-field="displayName"
          placeholder="ابحث باسم الطالب أو الخطة"
          :loading="enrollmentLoading"
          :error="enrollmentError"
          empty-message="لا توجد اشتراكات مؤهلة لهذه المجموعة"
          @select="selectEnrollment"
          @clear="clearEnrollment"
          @search="$emit('onSearchAcademicEnrollment', $event)"
        />
        <ValidationError :messages="errors?.studentAcademicEnrollmentID" />
      </div>

      <div
        v-if="selectedAcademicEnrollment.item"
        class="rounded-lg border border-[var(--border-color)] bg-[var(--bg-body)] p-3 text-sm"
      >
        <p>
          <strong>الطالب:</strong>
          {{ selectedAcademicEnrollment.item.studentName }}
        </p>
        <p>
          <strong>الخطة:</strong>
          {{ selectedAcademicEnrollment.item.academicPlanName }} (v{{
            selectedAcademicEnrollment.item.academicPlanVersion
          }})
        </p>
        <p>
          <strong>عناصر الخطة:</strong>
          {{ selectedAcademicEnrollment.item.itemCount }}
        </p>
        <p>
          <strong>الصلاحية:</strong>
          {{ selectedAcademicEnrollment.item.validFrom }} —
          {{ selectedAcademicEnrollment.item.validTo || "مفتوحة" }}
        </p>
      </div>

      <div class="form-group">
        <label>تاريخ بدء العضوية</label>
        <input v-model="formData.date" type="date" />
        <ValidationError :messages="errors?.date" />
      </div>
      <button
        type="submit"
        class="btn btn-primary"
        :disabled="
          !formData.studentAcademicEnrollmentID ||
          (!formData.groupID &&
            !props.groupData?.groupID &&
            !props.groupData?.GroupID &&
            !props.groupData?.groupId) ||
          enrollmentLoading
        "
      >
        إضافة إلى المجموعة
      </button>
    </form>
  </section>
</template>
