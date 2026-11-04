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
              اسم الصف
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              المستوى
            </th>
            <th
              class="px-5 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
            >
              البرنامج
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
            v-for="cls in classes"
            :key="cls.classID"
            class="transition hover:bg-gray-50/50 dark:hover:bg-white/2"
          >
            <td v-if="canSelect" class="px-5 py-4">
              <input
                type="checkbox"
                :checked="selectedIds.includes(cls.classID)"
                @change="$emit('toggle-select-row', cls)"
              />
            </td>
            <td
              class="px-5 py-4 text-sm font-medium text-gray-800 dark:text-white/90"
            >
              {{ cls.name }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ cls.level }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ cls.programName }}
            </td>
            <td class="px-5 py-4 text-sm text-gray-500 dark:text-gray-400">
              {{ cls.notes }}
            </td>
            <td class="py-3 pe-5 text-end">
              <div class="flex justify-end gap-1">
                <button
                  v-if="canRead"
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-white/5"
                  @click="$emit('show-info-class', cls)"
                >
                  معلومات
                </button>
                <button
                  v-if="canUpdate"
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-brand-500 transition-colors hover:bg-brand-50 dark:text-brand-400 dark:hover:bg-brand-500/10"
                  @click="$emit('show-update-class', cls)"
                >
                  تحديث
                </button>
                <button
                  v-if="canCreateInstituteClass"
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-white/5"
                  @click="$emit('show-add-to-institue-class', cls)"
                >
                  إضافة إلى معهد
                </button>
                <button
                  v-if="canDelete"
                  type="button"
                  class="rounded-lg px-2.5 py-1.5 text-xs font-medium text-red-500 transition-colors hover:bg-red-50 dark:text-red-400 dark:hover:bg-red-500/10"
                  @click="$emit('show-delete-class', cls)"
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
  classes: {
    type: Array,
    required: true,
  },
  selectedIds: {
    type: Array,
    default: () => [],
  },
  canRead: { type: Boolean, default: false },
  canUpdate: { type: Boolean, default: false },
  canDelete: { type: Boolean, default: false },
  canCreateInstituteClass: { type: Boolean, default: false },
  canSelect: { type: Boolean, default: false },
});

defineEmits([
  "show-update-class",
  "show-delete-class",
  "show-info-class",
  "show-add-to-institue-class",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(() => {
  return (
    props.classes.length > 0 &&
    props.classes.every((c) => props.selectedIds.includes(c.classID))
  );
});
</script>
