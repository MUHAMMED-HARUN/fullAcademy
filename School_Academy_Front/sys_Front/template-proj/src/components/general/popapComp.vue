<template>
  <div
    class="fixed inset-0 bg-black/50 backdrop-blur-xs dark:bg-black/70"
    aria-hidden="true"
    @click="emit('close')"
  />
  <div
    class="fixed start-1/2 top-1/2 z-101 w-[90%] max-w-lg -translate-x-1/2 -translate-y-1/2 rounded-2xl border border-gray-200 bg-white p-6 shadow-theme-lg dark:border-gray-800 dark:bg-[#1E2635] rtl:translate-x-1/2"
    ref="dialog"
    role="dialog"
    aria-modal="true"
    aria-label="نافذة حوارية"
    tabindex="-1"
    @keydown.esc.stop="emit('close')"
  >
    <div class="max-h-[80vh] overflow-y-auto">
      <slot />
    </div>
    <div
      class="mt-4 flex justify-end border-t border-gray-100 pt-4 dark:border-gray-800"
    >
      <button
        type="button"
        class="h-9 rounded-[10px] border border-gray-300 px-4 text-sm font-medium text-gray-700 shadow-theme-xs transition-colors hover:bg-gray-50 dark:border-gray-700 dark:text-gray-300 dark:hover:bg-white/5"
        @click="emit('close')"
      >
        إغلاق
      </button>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";

const emit = defineEmits(["close"]);
const dialog = ref(null);

onMounted(() => dialog.value?.focus());
</script>
