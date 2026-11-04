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
              اسم المجموعة
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              كود المجموعة
            </th>
            <th
              class="px-5 py-3 text-end text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              إجراءات
            </th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100 dark:divide-white/5">
          <tr
            v-for="group in groups"
            :key="group.groupID"
            class="transition hover:bg-gray-50/50 dark:hover:bg-white/2"
          >
            <td class="px-5 py-4">
              <input
                type="checkbox"
                :checked="selectedIds.includes(group.groupID)"
                @change="$emit('toggle-select-row', group)"
              />
            </td>
            <td
              class="px-5 py-4 text-sm font-medium text-gray-800 dark:text-white/90"
            >
              {{ group.groupName }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ group.code }}
            </td>
            <td class="py-3 pe-5 text-end">
              <div class="flex justify-end gap-1">
                <button
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-white/5"
                  @click="$emit('show-info-group', group)"
                >
                  معلومات
                </button>
                <button
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-brand-500 transition-colors hover:bg-brand-50 dark:text-brand-400 dark:hover:bg-brand-500/10"
                  @click="$emit('show-update-group', group)"
                >
                  تحديث
                </button>
                <button
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-red-500 transition-colors hover:bg-red-50 dark:text-red-400 dark:hover:bg-red-500/10"
                  @click="$emit('show-delete-group', group)"
                >
                  حذف
                </button>
              </div>
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
  groups: {
    type: Array,
    required: true,
  },
  selectedIds: {
    type: Array,
    default: () => [],
  },
});

defineEmits([
  "show-update-group",
  "show-delete-group",
  "show-info-group",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(() => {
  return (
    props.groups.length > 0 &&
    props.groups.every((g) => props.selectedIds.includes(g.groupID))
  );
});
</script>
