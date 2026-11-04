<template>
  <section class="space-y-4" dir="rtl" aria-labelledby="district-card-title">
    <h2
      id="district-card-title"
      class="text-xl font-bold text-[var(--primary-color)]"
    >
      تفاصيل المنطقة
    </h2>
    <dl class="grid grid-cols-1 gap-3 sm:grid-cols-2">
      <div class="rounded-lg border border-[var(--border-color)] p-3">
        <dt class="text-sm text-[var(--text-muted)]">المنطقة</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{ district?.name || "—" }}
        </dd>
      </div>
      <div class="rounded-lg border border-[var(--border-color)] p-3">
        <dt class="text-sm text-[var(--text-muted)]">المدينة</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{ district?.cityName || "—" }}
        </dd>
      </div>
      <div
        class="rounded-lg border border-[var(--border-color)] p-3 sm:col-span-2"
      >
        <dt class="text-sm text-[var(--text-muted)]">الدولة</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{ district?.countryName || "—" }}
        </dd>
      </div>
    </dl>
  </section>
</template>

<script setup>
defineProps({ district: { type: Object, default: null } });
</script>
