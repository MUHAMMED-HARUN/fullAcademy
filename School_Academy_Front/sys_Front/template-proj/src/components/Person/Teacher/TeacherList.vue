<template>
  <section class="space-y-4" dir="rtl" aria-label="قائمة المعلمين">
    <div
      v-if="props.loading"
      class="flex min-h-32 items-center justify-center gap-3 rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)] p-6 text-[var(--text-muted)]"
      role="status"
      aria-live="polite"
    >
      <span
        class="h-5 w-5 animate-spin rounded-full border-2 border-[var(--primary-color-light)] border-t-[var(--primary-color)]"
        aria-hidden="true"
      />
      <span>جارٍ تحميل بيانات المعلمين...</span>
    </div>

    <div
      v-else-if="errorMessage"
      class="rounded-xl border border-[var(--danger-color)] bg-[var(--danger-color-light)] p-5 text-[var(--danger-color)]"
      role="alert"
    >
      {{ errorMessage }}
    </div>

    <div
      v-else-if="props.teachers.length === 0"
      class="rounded-xl border border-dashed border-[var(--border-color)] bg-[var(--bg-card)] px-6 py-12 text-center"
      role="status"
    >
      <h3 class="text-base font-semibold text-[var(--primary-color)]">
        لا توجد بيانات معلمين
      </h3>
      <p class="mt-2 text-sm text-[var(--text-muted)]">
        أضف معلماً جديداً أو غيّر معايير البحث لعرض النتائج.
      </p>
    </div>

    <div
      v-else
      class="overflow-hidden rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)] shadow-sm"
    >
      <div class="max-w-full overflow-x-auto">
        <table class="min-w-[46rem] w-full text-start">
          <caption class="sr-only">
            قائمة المعلمين
          </caption>
          <thead
            class="border-b border-[var(--border-color)] bg-[var(--bg-body)]"
          >
            <tr>
              <th
                class="px-4 py-3 text-start text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                <input
                  type="checkbox"
                  :checked="isAllSelected"
                  aria-label="تحديد جميع المعلمين في الصفحة"
                  @change="$emit('toggle-select-all', $event.target.checked)"
                />
              </th>
              <th
                class="px-4 py-3 text-start text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                الاسم
              </th>
              <th
                class="px-4 py-3 text-start text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                الكنية
              </th>
              <th
                class="px-4 py-3 text-start text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                الرقم الوطني
              </th>
              <th
                class="px-4 py-3 text-start text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                الجنس
              </th>
              <th
                class="px-4 py-3 text-start text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                مكان الإقامة
              </th>
              <th
                class="px-4 py-3 text-end text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                الإجراءات
              </th>
            </tr>
          </thead>
          <tbody class="divide-y divide-[var(--border-color)]">
            <tr
              v-for="teacher in props.teachers"
              :key="teacher.teacherID"
              class="transition-colors hover:bg-[var(--bg-body)]"
              :class="{
                'bg-[var(--primary-color-light)]': props.selectedIds.includes(
                  teacher.teacherID
                ),
              }"
            >
              <td class="px-4 py-3 sm:px-5 sm:py-4">
                <input
                  type="checkbox"
                  :checked="props.selectedIds.includes(teacher.teacherID)"
                  :aria-label="`تحديد المعلم ${teacher.firstName || ''} ${
                    teacher.lastName || ''
                  }`"
                  @change="$emit('toggle-select-row', teacher)"
                />
              </td>
              <td
                class="px-4 py-3 text-sm font-semibold text-[var(--text-color)] sm:px-5 sm:py-4"
              >
                {{
                  [teacher.firstName, teacher.fatherName]
                    .filter(Boolean)
                    .join(" ") || "—"
                }}
              </td>
              <td
                class="px-4 py-3 text-sm text-[var(--text-muted)] sm:px-5 sm:py-4"
              >
                {{ teacher.lastName || "—" }}
              </td>
              <td
                class="px-4 py-3 text-sm text-[var(--text-muted)] sm:px-5 sm:py-4"
              >
                {{ teacher.nationalNumber || "—" }}
              </td>
              <td
                class="px-4 py-3 text-sm text-[var(--text-muted)] sm:px-5 sm:py-4"
              >
                {{ teacher.gender ? "ذكر" : "أنثى" }}
              </td>
              <td
                class="px-4 py-3 text-sm text-[var(--text-muted)] sm:px-5 sm:py-4"
              >
                {{ teacher.livingPlace || "—" }}
              </td>
              <td class="px-4 py-3 text-end sm:px-5 sm:py-4">
                <div class="flex justify-end gap-2">
                  <button
                    type="button"
                    class="btn btn-sm btn-outline-primary"
                    @click="$emit('show-info-teacher', teacher)"
                  >
                    معلومات
                  </button>
                  <button
                    type="button"
                    class="btn btn-sm btn-primary"
                    @click="$emit('show-update-teacher', teacher)"
                  >
                    تعديل
                  </button>
                  <button
                    type="button"
                    class="btn btn-sm btn-outline-danger"
                    @click="$emit('show-delete-teacher', teacher)"
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
  </section>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps({
  teachers: { type: Array, required: true },
  selectedIds: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: [String, Array, Object], default: null },
});

defineEmits([
  "show-update-teacher",
  "show-delete-teacher",
  "show-info-teacher",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(
  () =>
    props.teachers.length > 0 &&
    props.teachers.every((teacher) =>
      props.selectedIds.includes(teacher.teacherID)
    )
);

function flattenMessages(value) {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
}

const errorMessage = computed(() => flattenMessages(props.error).join("، "));
</script>
