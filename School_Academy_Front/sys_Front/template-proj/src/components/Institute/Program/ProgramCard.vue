<template>
  <div
    class="rounded-2xl border border-gray-200 bg-white p-5 dark:border-gray-800 dark:bg-white/3"
  >
    <div class="flex items-start justify-between gap-3">
      <div>
        <h4 class="text-base font-semibold text-gray-800 dark:text-white/90">
          {{ DataReq?.name || "—" }}
        </h4>
        <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">
          {{ DataReq?.note || "لا توجد ملاحظات" }}
        </p>
      </div>
      <span
        class="rounded-lg bg-brand-50 px-2.5 py-1 text-xs font-medium text-brand-500 dark:bg-brand-500/15 dark:text-brand-400"
      >
        Program
      </span>
    </div>
  </div>
</template>

<script setup>
defineProps({
  DataReq: {
    type: Object,
    required: true,
  },
});
</script>
