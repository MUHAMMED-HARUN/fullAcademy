<template>
  <div
    v-if="actions && actions.length > 0"
    class="flex flex-wrap items-center gap-3 rounded-xl border border-gray-200 bg-gray-50 px-4 py-3 dark:border-gray-800 dark:bg-white/5"
  >
    <span
      v-if="selectedCount > 0"
      class="text-sm font-semibold text-gray-700 dark:text-gray-300"
    >
      تم تحديد ({{ selectedCount }}) سجل
    </span>
    <button
      v-for="action in actions"
      :key="action.key"
      type="button"
      class="h-9 rounded-[10px] px-4 text-sm font-medium text-white shadow-theme-xs transition-colors disabled:cursor-not-allowed disabled:opacity-50"
      :class="action.class || 'bg-brand-500 hover:bg-brand-600'"
      :disabled="isActionDisabled(action)"
      @click="$emit('action-click', action.key, selectedItems)"
    >
      {{ action.label }}
    </button>
  </div>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps({
  actions: {
    type: Array,
    required: true,
  },
  selectedItems: {
    type: Array,
    default: () => [],
  },
});

defineEmits(["action-click"]);

const selectedCount = computed(() => props.selectedItems.length);

function isActionDisabled(action) {
  const count = selectedCount.value;

  if (action.minSelection !== undefined && count < action.minSelection)
    return true;
  if (action.maxSelection !== undefined && count > action.maxSelection)
    return true;
  if (action.minSelection === undefined && count === 0) return true;

  if (action.condition && count > 0) {
    return !props.selectedItems.every((item) => action.condition(item));
  }

  return false;
}
</script>
