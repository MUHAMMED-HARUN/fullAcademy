<template>
  <div
    class="rounded-2xl border border-gray-200 bg-white p-5 dark:border-gray-800 dark:bg-white/3"
  >
    <div class="flex items-start justify-between gap-3">
      <div>
        <h4 class="text-base font-semibold text-gray-800 dark:text-white/90">
          {{ DataReq?.groupName || "—" }}
        </h4>
        <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">
          {{ DataReq?.code || "لا يوجد كود" }}
        </p>
        <p class="mt-2 text-sm text-gray-600 dark:text-gray-300">
          {{ DataReq?.className || "—" }} — {{ DataReq?.instituteName || "—" }}
        </p>
        <p
          v-if="DataReq?.hasActiveRelationships"
          class="mt-2 text-xs text-[var(--text-muted)]"
        >
          تحتوي المجموعة على عضويات أو تكليفات نشطة؛ لا يمكن حذفها أو تغيير
          صفها.
        </p>
      </div>
      <span
        class="rounded-lg bg-brand-50 px-2.5 py-1 text-xs font-medium text-brand-500 dark:bg-brand-500/15 dark:text-brand-400"
      >
        Group
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
