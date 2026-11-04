<template>
  <section class="space-y-4" dir="rtl" aria-label="قائمة الطلاب">
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
      <span>جارٍ تحميل بيانات الطلاب...</span>
    </div>

    <div
      v-else-if="errorMessage"
      class="rounded-xl border border-[var(--danger-color)] bg-[var(--danger-color-light)] p-5 text-[var(--danger-color)]"
      role="alert"
    >
      {{ errorMessage }}
    </div>

    <div
      v-else-if="props.students.length === 0"
      class="rounded-xl border border-dashed border-[var(--border-color)] bg-[var(--bg-card)] px-6 py-12 text-center"
      role="status"
    >
      <h3 class="text-base font-semibold text-[var(--primary-color)]">
        لا توجد بيانات طلاب
      </h3>
      <p class="mt-2 text-sm text-[var(--text-muted)]">
        أضف طالباً جديداً أو غيّر معايير البحث لعرض النتائج.
      </p>
    </div>

    <div
      v-else
      class="overflow-hidden rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)] shadow-sm"
    >
      <div class="max-w-full overflow-x-auto">
        <table class="min-w-[46rem] w-full text-start">
          <caption class="sr-only">
            قائمة الطلاب
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
                  aria-label="تحديد جميع الطلاب في الصفحة"
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
                class="px-4 py-3 text-end text-xs font-semibold text-[var(--text-muted)] sm:px-5"
              >
                الإجراءات
              </th>
            </tr>
          </thead>
          <tbody class="divide-y divide-[var(--border-color)]">
            <tr
              v-for="student in props.students"
              :key="student.studentID"
              class="transition-colors hover:bg-[var(--bg-body)]"
            >
              <td class="px-4 py-3 sm:px-5 sm:py-4">
                <input
                  type="checkbox"
                  :checked="props.selectedIds.includes(student.studentID)"
                  :aria-label="`تحديد الطالب ${
                    student.displayName || student.firstName
                  }`"
                  @change="$emit('toggle-select-row', student)"
                />
              </td>
              <td
                class="px-4 py-3 text-sm font-semibold text-[var(--text-color)] sm:px-5 sm:py-4"
              >
                {{
                  student.displayName ||
                  [student.firstName, student.fatherName]
                    .filter(Boolean)
                    .join(" ")
                }}
              </td>
              <td
                class="px-4 py-3 text-sm text-[var(--text-muted)] sm:px-5 sm:py-4"
              >
                {{ student.lastName || "—" }}
              </td>
              <td
                class="px-4 py-3 text-sm text-[var(--text-muted)] sm:px-5 sm:py-4"
              >
                {{ student.nationalNumber || "—" }}
              </td>
              <td
                class="px-4 py-3 text-sm text-[var(--text-muted)] sm:px-5 sm:py-4"
              >
                {{ student.gender ? "ذكر" : "أنثى" }}
              </td>
              <td class="px-4 py-3 text-end sm:px-5 sm:py-4">
                <div class="flex justify-end gap-2">
                  <button
                    type="button"
                    class="btn btn-sm btn-outline-primary"
                    @click="$emit('show-info-student', student)"
                  >
                    معلومات
                  </button>
                  <button
                    type="button"
                    class="btn btn-sm btn-primary"
                    @click="$emit('show-update-student', student)"
                  >
                    تعديل
                  </button>
                  <button
                    type="button"
                    class="btn btn-sm btn-outline-danger"
                    @click="$emit('show-delete-student', student)"
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
  students: { type: Array, required: true },
  selectedIds: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: [String, Array, Object], default: null },
});

defineEmits([
  "show-update-student",
  "show-delete-student",
  "show-info-student",
  "toggle-select-all",
  "toggle-select-row",
]);

const isAllSelected = computed(
  () =>
    props.students.length > 0 &&
    props.students.every((student) =>
      props.selectedIds.includes(student.studentID)
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
