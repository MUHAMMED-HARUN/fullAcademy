<template>
  <section class="space-y-5" dir="rtl" aria-labelledby="teacher-form-title">
    <header>
      <h2
        id="teacher-form-title"
        class="text-xl font-bold text-[var(--primary-color)]"
      >
        {{ formTitle }}
      </h2>
      <p class="mt-1 text-sm text-[var(--text-muted)]">
        اربط سجل المعلم بشخص موجود في النظام.
      </p>
    </header>

    <form class="space-y-4" @submit.prevent="submit">
      <div v-if="props.personLocked" class="space-y-3">
        <p
          class="rounded-lg border border-[var(--border-color)] bg-[var(--bg-body)] p-3 text-sm text-[var(--text-muted)]"
        >
          تم تثبيت الشخص من صفحة الأشخاص ولا يمكن تغييره هنا.
        </p>
        <PersonCard v-if="selectedPerson" :data-req="selectedPerson" />
        <p v-else class="alert-error" role="alert">
          تعذر تحميل بيانات الشخص المرتبط.
        </p>
      </div>

      <div v-else class="form-group space-y-3">
        <span class="block font-medium">الشخص المرتبط بالمعلم</span>
        <p v-if="formData.teacherID" class="text-sm text-[var(--text-muted)]">
          يمكنك تغيير الشخص المرتبط؛ سيُحدّث هذا سجل المعلم القائم.
        </p>
        <GenericSearchByFilter
          v-model="personID"
          v-model:name-value="selectedPersonName"
          :filter-types="personFilterTypes"
          :results="props.personSearchResults"
          :loading="props.personSearchLoading"
          :error="props.personSearchError"
          id-field="id"
          name-field="displayName"
          placeholder="ابحث بالاسم أو الرقم الوطني"
          empty-message="لم يتم العثور على أشخاص مطابقين"
          @search="emit('search-person', $event)"
          @select="onPersonSelected"
          @clear="onPersonCleared"
          @query-change="emit('person-query-change', $event)"
        />
        <ValidationError :messages="props.errors?.personID" />
        <PersonCard v-if="selectedPerson" :data-req="selectedPerson" />
        <p v-else class="text-sm text-[var(--text-muted)]" role="status">
          اختر نتيجة البحث لإظهار بطاقة الشخص.
        </p>
      </div>

      <p v-if="generalError" class="alert-error" role="alert">
        {{ generalError }}
      </p>

      <footer
        class="flex flex-wrap items-center justify-end gap-3 border-t border-[var(--border-color)] pt-4"
      >
        <span
          v-if="props.saving"
          class="text-sm text-[var(--text-muted)]"
          role="status"
        >
          جارٍ حفظ بيانات المعلم...
        </span>
        <button
          type="submit"
          class="btn btn-primary"
          :disabled="props.saving || !formData.personID || !selectedPerson"
        >
          {{ props.saving ? "جارٍ الحفظ" : "حفظ المعلم" }}
        </button>
      </footer>
    </form>
  </section>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import PersonCard from "@/components/Person/Person/PersonCard.vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  modelValue: {
    type: Object,
    default: () => ({ teacherID: "", personID: "" }),
  },
  personData: {
    type: Object,
    default: null,
  },
  personSearchResults: {
    type: Array,
    default: () => [],
  },
  personSearchLoading: {
    type: Boolean,
    default: false,
  },
  personSearchError: {
    type: [String, Array, Object],
    default: null,
  },
  errors: {
    type: [String, Array, Object],
    default: () => ({}),
  },
  saving: {
    type: Boolean,
    default: false,
  },
  personLocked: {
    type: Boolean,
    default: false,
  },
});

const emit = defineEmits([
  "update:modelValue",
  "submit",
  "search-person",
  "select-person",
  "clear-person",
  "person-query-change",
]);

const formData = reactive({ teacherID: "", personID: "" });
const selectedPerson = ref(null);
const selectedPersonName = ref("");
const personFilterTypes = [
  { value: "FirstName", label: "الاسم" },
  { value: "NationalNumber", label: "الرقم الوطني" },
];

const formTitle = computed(() =>
  formData.teacherID ? "تعديل بيانات المعلم" : "إضافة معلم"
);
const generalError = computed(() => {
  const value = props.errors;
  if (typeof value === "string") return value;
  if (Array.isArray(value)) return value.join("، ");
  if (!value || typeof value !== "object") return "";

  return Object.entries(value)
    .filter(([key]) => key !== "personID" && key !== "_form")
    .flatMap(([, message]) => flattenErrorMessages(message))
    .join("، ");
});
const personID = computed({
  get: () => formData.personID,
  set: (value) => {
    formData.personID = value || "";
    emit("update:modelValue", { ...formData });
  },
});

function personIdOf(person) {
  return person?.id || person?.personID || "";
}

function personNameOf(person) {
  return (
    person?.displayName ||
    [person?.firstName, person?.fatherName, person?.lastName]
      .filter(Boolean)
      .join(" ")
  );
}

function flattenErrorMessages(value) {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenErrorMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenErrorMessages);
  }
  return [];
}

watch(
  () => props.modelValue,
  (value) => {
    formData.teacherID = value?.teacherID || "";
    formData.personID = value?.personID || "";
  },
  { immediate: true, deep: true }
);

watch(
  () => props.personData,
  (person) => {
    selectedPerson.value = person && personIdOf(person) ? person : null;
    selectedPersonName.value = personNameOf(selectedPerson.value);

    if (props.personLocked && selectedPerson.value) {
      formData.personID = personIdOf(selectedPerson.value);
      emit("update:modelValue", { ...formData });
    }
  },
  { immediate: true, deep: true }
);

function onPersonSelected(person) {
  selectedPerson.value = person;
  selectedPersonName.value = personNameOf(person);
  formData.personID = personIdOf(person);
  emit("update:modelValue", { ...formData });
  emit("select-person", person);
}

function onPersonCleared() {
  selectedPerson.value = null;
  selectedPersonName.value = "";
  emit("clear-person");
}

function submit() {
  if (!formData.personID || !selectedPerson.value || props.saving) return;
  emit("submit", { ...formData });
}
</script>
