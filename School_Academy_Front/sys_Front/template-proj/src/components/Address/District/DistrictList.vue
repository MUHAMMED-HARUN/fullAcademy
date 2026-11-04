<template>
  <section class="space-y-3" dir="rtl" aria-label="قائمة المناطق">
    <p v-if="loading" class="text-sm text-[var(--text-muted)]" role="status">
      جارٍ تحميل المناطق...
    </p>
    <p v-else-if="errorText" class="alert-error" role="alert">
      {{ errorText }}
    </p>
    <p
      v-else-if="districts.length === 0"
      class="rounded-xl border border-dashed border-[var(--border-color)] p-6 text-center text-sm text-[var(--text-muted)]"
      role="status"
    >
      لا توجد مناطق مطابقة.
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
                  aria-label="تحديد كل المناطق في الصفحة"
                  @change="$emit('toggle-select-all', $event.target.checked)"
                />
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
              v-for="district in districts"
              :key="district.id"
              class="transition hover:bg-[var(--bg-body)]"
            >
              <td class="px-5 py-4">
                <input
                  type="checkbox"
                  :checked="selectedIds.includes(district.id)"
                  :aria-label="`تحديد ${district.name}`"
                  @change="$emit('toggle-select-row', district)"
                />
              </td>
              <td
                class="px-5 py-4 text-sm font-medium text-[var(--text-color)]"
              >
                {{ district.name }}
              </td>
              <td class="px-5 py-4 text-sm text-[var(--text-muted)]">
                {{ district.cityName }}
              </td>
              <td class="px-5 py-4 text-sm text-[var(--text-muted)]">
                {{ district.countryName }}
              </td>
              <td class="py-3 pe-5 text-end">
                <div class="flex justify-end gap-1">
                  <button
                    type="button"
                    class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-[var(--text-color)] transition-colors hover:bg-[var(--bg-body)] focus-visible:outline focus-visible:outline-2 focus-visible:outline-[var(--primary-color)]"
                    @click="$emit('show-info-district', district)"
                  >
                    معلومات
                  </button>
                  <button
                    type="button"
                    class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-[var(--primary-color)] transition-colors hover:bg-[var(--primary-color-light)] focus-visible:outline focus-visible:outline-2 focus-visible:outline-[var(--primary-color)]"
                    @click="$emit('show-update-district', district)"
                  >
                    تحديث
                  </button>
                </div>
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
  districts: { type: Array, required: true },
  selectedIds: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: [String, Array, Object], default: null },
});

defineEmits([
  "show-update-district",
  "show-info-district",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(
  () =>
    props.districts.length > 0 &&
    props.districts.every((district) => props.selectedIds.includes(district.id))
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
