<template>
  <section class="space-y-3" dir="rtl" aria-label="بيانات المعلم">
    <header
      class="flex flex-wrap items-center justify-between gap-3 rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)] p-4"
    >
      <div>
        <h2 class="text-lg font-bold text-[var(--primary-color)]">
          بيانات المعلم
        </h2>
        <p class="mt-1 text-sm text-[var(--text-muted)]">
          رقم المعلم:
          <span class="font-medium text-[var(--text-color)]">{{
            props.dataReq?.teacherID || "—"
          }}</span>
        </p>
      </div>
      <span
        class="rounded-full bg-[var(--primary-color-light)] px-3 py-1 text-xs font-semibold text-[var(--primary-color)]"
      >
        معلم
      </span>
    </header>
    <PersonCard :data-req="props.dataReq" />
  </section>
</template>

<script setup>
import PersonCard from "@/components/Person/Person/PersonCard.vue";

const props = defineProps({
  dataReq: { type: Object, default: null },
});
</script>
