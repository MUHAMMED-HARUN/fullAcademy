<template>
  <section class="space-y-4" dir="rtl" aria-label="ربط صفوف متعددة بمعهد">
    <header class="space-y-1">
      <h2 class="text-xl font-bold text-[var(--primary-color)]">
        إضافة عدة صفوف إلى معهد
      </h2>
      <p class="text-sm text-[var(--text-muted)]">
        الصفوف المرتبطة مسبقاً بالمعهد سيتم تجاوزها عند الحفظ.
      </p>
    </header>

    <form class="space-y-4" @submit.prevent="onSubmit">
      <div class="form-group">
        <label class="mb-1 block font-medium">المعهد:</label>
        <GenericSearchByFilter
          v-model="instituteID"
          v-model:name-value="instituteName"
          :filter-types="instituteFilterTypes"
          :results="props.instituteResults"
          :loading="props.instituteSearchLoading"
          :error="props.instituteSearchError"
          id-field="instituteID"
          name-field="name"
          placeholder="ابحث عن معهد بالاسم"
          empty-message="لم يتم العثور على معهد مطابق"
          @search="emit('search-institute', $event)"
          @select="onInstituteSelected"
          @clear="clearInstituteSelection"
        />
        <ValidationError :messages="props.errors?.instituteID" />
      </div>

      <fieldset
        class="space-y-2 rounded-xl border border-[var(--border-color)] p-4"
      >
        <legend class="px-2 font-semibold text-[var(--primary-color)]">
          الصفوف المختارة ({{ props.selectedClasses.length }})
        </legend>
        <ul v-if="props.selectedClasses.length" class="space-y-2">
          <li
            v-for="cls in props.selectedClasses"
            :key="cls.classID"
            class="rounded-lg border border-[var(--border-color)] px-3 py-2 text-sm"
          >
            <span class="font-medium">{{ cls.name }}</span>
            <span v-if="cls.programName" class="text-[var(--text-muted)]">
              — {{ cls.programName }}
            </span>
          </li>
        </ul>
        <p v-else class="text-sm text-[var(--text-muted)]" role="status">
          لم يتم اختيار أي صف.
        </p>
      </fieldset>

      <ValidationError :messages="formErrors" />

      <div class="flex justify-end border-t border-[var(--border-color)] pt-4">
        <button
          class="btn btn-primary min-w-32"
          type="submit"
          :disabled="
            props.loading || !selectedInstitute || !props.selectedClasses.length
          "
          :aria-busy="props.loading"
        >
          <span
            v-if="props.loading"
            class="inline-block h-4 w-4 animate-spin rounded-full border-2 border-white/40 border-t-white"
            aria-hidden="true"
          />
          {{ props.loading ? "جارٍ الربط..." : "حفظ الربط" }}
        </button>
      </div>
    </form>
  </section>
</template>

<script setup>
import { computed, ref } from "vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  selectedClasses: { type: Array, default: () => [] },
  instituteResults: { type: Array, default: () => [] },
  instituteSearchLoading: { type: Boolean, default: false },
  instituteSearchError: { type: [String, Array, Object], default: null },
  loading: { type: Boolean, default: false },
  errors: { type: [Object, String, Array], default: () => ({}) },
});

const emit = defineEmits([
  "onSubmit",
  "search-institute",
  "clear-institute-search",
]);

const instituteID = ref("");
const instituteName = ref("");
const selectedInstitute = ref(null);
const instituteFilterTypes = [{ value: "Name", label: "اسم المعهد" }];
const formErrors = computed(() => {
  if (typeof props.errors === "string" || Array.isArray(props.errors)) {
    return props.errors;
  }
  return props.errors?._form || null;
});

function onInstituteSelected(institute) {
  selectedInstitute.value = institute;
  instituteID.value = institute.instituteID || "";
  instituteName.value = institute.name || "";
}

function clearInstituteSelection() {
  selectedInstitute.value = null;
  instituteID.value = "";
  instituteName.value = "";
  emit("clear-institute-search");
}

function onSubmit() {
  if (!selectedInstitute.value) return;
  const classIDs = [
    ...new Set(props.selectedClasses.map((cls) => cls.classID).filter(Boolean)),
  ];
  emit("onSubmit", {
    instituteID: instituteID.value,
    classIDs,
  });
}
</script>
