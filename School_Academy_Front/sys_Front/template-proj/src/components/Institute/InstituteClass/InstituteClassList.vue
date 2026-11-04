<template>
  <div
    class="overflow-hidden rounded-xl border border-gray-200 bg-white dark:border-white/5 dark:bg-white/3"
  >
    <div class="max-w-full overflow-x-auto">
      <table class="min-w-full">
        <thead class="border-b border-gray-100 dark:border-white/5">
          <tr>
            <th
              v-if="canSelect"
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
              المعهد
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              الصف
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              البرنامج
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              مستوى الصف
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
            v-for="ic in instituteClasses"
            :key="ic.instituteClassID"
            class="transition hover:bg-gray-50/50 dark:hover:bg-white/2"
          >
            <td v-if="canSelect" class="px-5 py-4">
              <input
                type="checkbox"
                :checked="selectedIds.includes(ic.instituteClassID)"
                @change="$emit('toggle-select-row', ic)"
              />
            </td>
            <td
              class="px-5 py-4 text-sm font-medium text-gray-800 dark:text-white/90"
            >
              {{ ic.instituteName }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ ic.className }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ ic.programName }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ ic.classLevel }}
            </td>
            <td class="py-3 pe-5 text-end">
              <div class="flex justify-end gap-1">
                <button
                  v-if="canRead"
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-white/5"
                  @click="$emit('show-info-institute-class', ic)"
                >
                  معلومات
                </button>
                <button
                  v-if="canAddGroup"
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-brand-500 transition-colors hover:bg-brand-50 dark:text-brand-400 dark:hover:bg-brand-500/10"
                  @click="$emit('show-add-group', ic)"
                >
                  إضافة مجموعة
                </button>
                <button
                  v-if="canDelete"
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-red-500 transition-colors hover:bg-red-50 dark:text-red-400 dark:hover:bg-red-500/10"
                  @click="$emit('show-delete-institute-class', ic)"
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
  instituteClasses: {
    type: Array,
    required: true,
  },
  selectedIds: {
    type: Array,
    default: () => [],
  },
  canRead: { type: Boolean, default: false },
  canDelete: { type: Boolean, default: false },
  canAddGroup: { type: Boolean, default: false },
  canSelect: { type: Boolean, default: false },
});

defineEmits([
  "show-delete-institute-class",
  "show-info-institute-class",
  "show-add-group",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(() => {
  return (
    props.instituteClasses.length > 0 &&
    props.instituteClasses.every((ic) =>
      props.selectedIds.includes(ic.instituteClassID)
    )
  );
});
</script>
