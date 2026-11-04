<template>
  <section class="space-y-3" dir="rtl" aria-label="قائمة الشوارع">
    <p v-if="loading" class="text-sm text-[var(--text-muted)]" role="status">
      جارٍ تحميل الشوارع...
    </p>
    <p v-else-if="errorText" class="alert-error" role="alert">
      {{ errorText }}
    </p>
    <p
      v-else-if="neighborhoods.length === 0"
      class="rounded-xl border border-dashed border-[var(--border-color)] p-6 text-center text-sm text-[var(--text-muted)]"
      role="status"
    >
      لا توجد شوارع مطابقة.
    </p>

    <div
      v-else
      class="overflow-hidden rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)]"
    >
      <div class="max-w-full overflow-x-auto">
        <table class="min-w-full">
          <thead class="border-b border-[var(--border-color)]">
            <tr>
              <th
                class="px-5 py-3 text-start text-xs font-medium text-[var(--text-muted)]"
              >
                <input
                  type="checkbox"
                  :checked="isAllSelected"
                  aria-label="تحديد كل الشوارع في الصفحة"
                  @change="$emit('toggle-select-all', $event.target.checked)"
                />
              </th>
              <th
                class="px-5 py-3 text-start text-xs font-medium text-[var(--text-muted)]"
              >
                الشارع
              </th>
              <th
                class="px-5 py-3 text-start text-xs font-medium text-[var(--text-muted)]"
              >
                المنطقة
              </th>
              <th
                class="px-5 py-3 text-start text-xs font-medium text-[var(--text-muted)]"
              >
                المدينة
              </th>
              <th
                class="px-5 py-3 text-start text-xs font-medium text-[var(--text-muted)]"
              >
                الدولة
              </th>
              <th
                class="px-5 py-3 text-end text-xs font-medium text-[var(--text-muted)]"
              >
                إجراءات
              </th>
            </tr>
          </thead>
          <tbody class="divide-y divide-[var(--border-color)]">
            <tr
              v-for="neighborhood in neighborhoods"
              :key="neighborhood.id"
              class="transition hover:bg-[var(--bg-body)]"
            >
              <td class="px-5 py-4">
                <input
                  type="checkbox"
                  :checked="selectedIds.includes(neighborhood.id)"
                  :aria-label="`تحديد ${neighborhood.name}`"
                  @change="$emit('toggle-select-row', neighborhood)"
                />
              </td>
              <td
                class="px-5 py-4 text-sm font-medium text-[var(--text-color)]"
              >
                {{ neighborhood.name }}
              </td>
              <td class="px-5 py-4 text-sm text-[var(--text-muted)]">
                {{ neighborhood.districtName }}
              </td>
              <td class="px-5 py-4 text-sm text-[var(--text-muted)]">
                {{ neighborhood.cityName }}
              </td>
              <td class="px-5 py-4 text-sm text-[var(--text-muted)]">
                {{ neighborhood.countryName }}
              </td>
              <td class="py-3 pe-5 text-end">
                <button
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-[var(--primary-color)] transition-colors hover:bg-[var(--primary-color-light)] focus-visible:outline focus-visible:outline-2 focus-visible:outline-[var(--primary-color)]"
                  @click="$emit('show-update-neighborhood', neighborhood)"
                >
                  تحديث
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </section>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps({
  neighborhoods: { type: Array, required: true },
  selectedIds: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: [String, Array, Object], default: null },
});

defineEmits([
  "show-update-neighborhood",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(
  () =>
    props.neighborhoods.length > 0 &&
    props.neighborhoods.every((item) => props.selectedIds.includes(item.id))
);

function flattenMessages(value) {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
}

const errorText = computed(() => flattenMessages(props.error).join("، "));
</script>
