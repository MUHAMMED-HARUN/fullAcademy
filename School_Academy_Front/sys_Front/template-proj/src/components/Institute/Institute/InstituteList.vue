<template>
  <div
    class="overflow-hidden rounded-xl border border-gray-200 bg-white dark:border-white/5 dark:bg-white/3"
  >
    <div class="max-w-full overflow-x-auto">
      <table class="min-w-full">
        <thead class="border-b border-gray-100 dark:border-white/5">
          <tr>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              <input
                type="checkbox"
                :checked="isAllSelected"
                @change="$emit('toggle-select-all', $event.target.checked)"
              />
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              اسم المعهد
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              عنوان المعهد
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              ملاحظات
            </th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100 dark:divide-white/5">
          <tr
            v-for="institute in institutes"
            :key="institute.instituteID"
            class="transition hover:bg-gray-50/50 dark:hover:bg-white/2"
          >
            <td class="px-5 py-4">
              <input
                type="checkbox"
                :checked="selectedIds.includes(institute.instituteID)"
                @change="$emit('toggle-select-row', institute)"
              />
            </td>
            <td
              class="px-5 py-4 text-sm font-medium text-gray-800 dark:text-white/90"
            >
              {{ institute.name }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ institute.address }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ institute.note }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps({
  institutes: {
    type: Array,
    required: true,
  },
  selectedIds: {
    type: Array,
    default: () => [],
  },
});

defineEmits([
  "show-update-institutes",
  "show-delete-institutes",
  "show-info-institutes",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(() => {
  return (
    props.institutes.length > 0 &&
    props.institutes.every((i) => props.selectedIds.includes(i.instituteID))
  );
});
</script>
