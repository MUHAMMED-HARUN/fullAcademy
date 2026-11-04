<template>
  <section class="space-y-4" dir="rtl" aria-labelledby="city-card-title">
    <header>
      <h2
        id="city-card-title"
        class="text-xl font-bold text-[var(--primary-color)]"
      >
        تفاصيل المدينة
      </h2>
    </header>

    <dl class="grid grid-cols-1 gap-3 sm:grid-cols-2">
      <div class="rounded-lg border border-[var(--border-color)] p-3">
        <dt class="text-sm text-[var(--text-muted)]">اسم المدينة</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{ city?.name || "—" }}
        </dd>
      </div>
      <div class="rounded-lg border border-[var(--border-color)] p-3">
        <dt class="text-sm text-[var(--text-muted)]">رمز المدينة</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{ city?.code || "—" }}
        </dd>
      </div>
      <div
        class="rounded-lg border border-[var(--border-color)] p-3 sm:col-span-2"
      >
        <dt class="text-sm text-[var(--text-muted)]">الدولة</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{ city?.countryName || "—" }}
        </dd>
      </div>
    </dl>
  </section>
</template>

<script setup>
defineProps({
  city: { type: Object, default: null },
});
</script>
