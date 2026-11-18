<template>
  <div class="table-responsive" dir="rtl">
    <table class="data-table">
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>الخطة الأكاديمية</th>
          <th>من</th>
          <th>إلى</th>
          <th>التاريخ</th>
          <th>مستوى التقييم</th>
          <th>الإجراءات</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="evaluation in dailyEvaluations"
          :key="evaluation.dailyEvaluationID"
        >
          <td>{{ evaluation.studentName }}</td>
          <td>{{ evaluation.academicPlanName }}</td>
          <td>{{ evaluation.from }}</td>
          <td>{{ evaluation.to }}</td>
          <td>{{ formatDate(evaluation.date) }}</td>
          <td>{{ evaluation.levelTypeName }}</td>
          <td class="flex flex-wrap gap-2">
            <button
              v-if="canRead"
              type="button"
              class="btn btn-sm btn-outline-secondary"
              @click="$emit('show-info', evaluation)"
            >
              معلومات
            </button>
            <button
              v-if="canUpdate"
              type="button"
              class="btn btn-sm btn-outline-primary"
              @click="$emit('show-update', evaluation)"
            >
              تحديث
            </button>
            <button
              v-if="canDelete"
              type="button"
              class="btn btn-sm btn-outline-danger"
              @click="$emit('show-delete', evaluation)"
            >
              حذف
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
defineProps({
  dailyEvaluations: { type: Array, required: true },
  canRead: { type: Boolean, default: false },
  canUpdate: { type: Boolean, default: false },
  canDelete: { type: Boolean, default: false },
});

defineEmits(["show-info", "show-update", "show-delete"]);

function formatDate(value) {
  if (!value) return "—";
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? value : date.toLocaleDateString("ar");
}
</script>
