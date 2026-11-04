<template>
  <section
    class="card card-hover space-y-5 p-5 sm:p-6"
    dir="rtl"
    aria-label="تفاصيل الشخص"
  >
    <header class="flex flex-wrap items-start justify-between gap-4">
      <div class="min-w-0">
        <h2 class="break-words text-lg font-bold text-[var(--primary-color)]">
          {{ props.dataReq?.firstName }} {{ props.dataReq?.fatherName }}
          {{ props.dataReq?.lastName }}
        </h2>
        <p class="mt-1 break-all text-sm text-[var(--text-muted)]">
          {{ props.dataReq?.nationalNumber || "لا يوجد رقم وطني" }}
        </p>
      </div>
      <span
        class="rounded-full bg-[var(--primary-color-light)] px-3 py-1 text-xs font-semibold text-[var(--primary-color)]"
      >
        شخص
      </span>
    </header>

    <dl class="grid grid-cols-2 gap-x-5 gap-y-4 text-sm">
      <div class="min-w-0">
        <dt class="text-xs text-[var(--text-muted)]">اسم الأم</dt>
        <dd class="mt-1 break-words font-medium text-[var(--text-color)]">
          {{
            [props.dataReq?.motherName, props.dataReq?.motherLastName]
              .filter(Boolean)
              .join(" ") || "—"
          }}
        </dd>
      </div>
      <div>
        <dt class="text-xs text-[var(--text-muted)]">الجنس</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{
            props.dataReq?.gender === undefined
              ? "—"
              : props.dataReq.gender
              ? "ذكر"
              : "أنثى"
          }}
        </dd>
      </div>
      <div>
        <dt class="text-xs text-[var(--text-muted)]">تاريخ الميلاد</dt>
        <dd class="mt-1 font-medium text-[var(--text-color)]">
          {{ birthDate }}
        </dd>
      </div>
      <div class="min-w-0">
        <dt class="text-xs text-[var(--text-muted)]">مكان الولادة</dt>
        <dd class="mt-1 break-words font-medium text-[var(--text-color)]">
          {{ props.dataReq?.placeOfBirth || "—" }}
        </dd>
      </div>
      <div class="col-span-2 min-w-0">
        <dt class="text-xs text-[var(--text-muted)]">مكان الإقامة</dt>
        <dd class="mt-1 break-words font-medium text-[var(--text-color)]">
          {{ props.dataReq?.livingPlace || "—" }}
        </dd>
      </div>
    </dl>
  </section>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps({
  dataReq: {
    type: Object,
    default: null,
  },
});

const birthDate = computed(() => {
  const value = props.dataReq?.birthDate;
  if (!value) return "—";

  const [year, month, day] = String(value).slice(0, 10).split("-").map(Number);
  if (!year || !month || !day) return value;

  return new Intl.DateTimeFormat("ar", { dateStyle: "medium" }).format(
    new Date(year, month - 1, day)
  );
});
</script>
