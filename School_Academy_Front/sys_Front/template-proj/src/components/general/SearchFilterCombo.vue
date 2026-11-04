<template>
  <GenericSearchByFilter
    v-bind="$attrs"
    :filter-types="filterTypes"
    :model-value="modelValue"
    :name-value="nameValue"
    :results="results"
    :id-field="idField"
    :name-field="nameField"
    :placeholder="placeholder"
    :loading="loading"
    :error="error"
    :empty-message="emptyMessage"
    @update:model-value="$emit('update:modelValue', $event)"
    @update:name-value="$emit('update:nameValue', $event)"
    @search="$emit('search', $event)"
    @select="$emit('select', $event)"
    @clear="$emit('clear')"
    @query-change="$emit('query-change', $event)"
    @no-results="$emit('no-results')"
  />
</template>

<script setup>
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";

defineProps({
  filterTypes: { type: Array, default: () => [] },
  modelValue: { type: [String, Number], default: "" },
  nameValue: { type: String, default: "" },
  results: { type: Array, default: () => [] },
  idField: { type: String, default: "id" },
  nameField: { type: String, default: "name" },
  placeholder: { type: String, default: "أدخل قيمة البحث" },
  loading: { type: Boolean, default: false },
  error: { type: [String, Array, Object], default: null },
  emptyMessage: { type: String, default: "لا توجد نتائج مطابقة" },
});

defineEmits([
  "update:modelValue",
  "update:nameValue",
  "search",
  "select",
  "clear",
  "query-change",
  "no-results",
]);
</script>
