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
              اسم المشروع
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
            v-for="program in programs"
            :key="program.programID"
            class="transition hover:bg-gray-50/50 dark:hover:bg-white/2"
          >
            <td class="px-5 py-4">
              <input
                type="checkbox"
                :checked="selectedIds.includes(program.programID)"
                @change="$emit('toggle-select-row', program)"
              />
            </td>
            <td
              class="px-5 py-4 text-sm font-medium text-gray-800 dark:text-white/90"
            >
              {{ program.name }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ program.note }}
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
  programs: {
    type: Array,
    required: true,
  },
  selectedIds: {
    type: Array,
    default: () => [],
  },
});

defineEmits([
  "show-update-program",
  "show-delete-program",
  "show-info-program",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(() => {
  return (
    props.programs.length > 0 &&
    props.programs.every((p) => props.selectedIds.includes(p.programID))
  );
});
</script>
