<script setup>
import {
  AcademicPlanScopeTypeOptions,
  AcademicPlanStatus,
  AcademicPlanStatusOptions,
} from "@/Generic/AcademicPlanEnums";
import { getEnumLabel } from "@/Generic/FinanceEnums";

defineProps({
  items: {
    type: Array,
    required: true,
  },
  canRead: {
    type: Boolean,
    default: false,
  },
  canUpdate: {
    type: Boolean,
    default: false,
  },
  canActivate: {
    type: Boolean,
    default: false,
  },
  canDelete: {
    type: Boolean,
    default: false,
  },
});

defineEmits(["info", "update", "activate", "delete"]);
</script>

<template>
  <div class="table-responsive">
    <table class="data-table">
      <thead>
        <tr>
          <th>اسم الخطة</th>
          <th>المؤسسة</th>
          <th>نوع النطاق</th>
          <th>النطاق</th>
          <th>الإصدار</th>
          <th>الحالة</th>
          <th>من</th>
          <th>إلى</th>
          <th>الإجراءات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.academicPlanID">
          <td>{{ item.name }}</td>
          <td>{{ item.providerInstituteName }}</td>
          <td>
            {{ getEnumLabel(AcademicPlanScopeTypeOptions, item.scopeType) }}
          </td>
          <td>{{ item.scopeName }}</td>
          <td>{{ item.version }}</td>
          <td>{{ getEnumLabel(AcademicPlanStatusOptions, item.status) }}</td>
          <td>{{ item.effectiveFrom }}</td>
          <td>{{ item.effectiveTo }}</td>
          <td class="actions-cell">
            <button
              v-if="canRead"
              type="button"
              class="btn-link"
              @click="$emit('info', item)"
            >
              عرض
            </button>
            <button
              v-if="canUpdate"
              type="button"
              class="btn-link"
              @click="$emit('update', item)"
            >
              تعديل
            </button>
            <button
              v-if="canActivate && item.status === AcademicPlanStatus.Draft"
              type="button"
              class="btn-link"
              @click="$emit('activate', item)"
            >
              تفعيل
            </button>
            <button
              v-if="canDelete && item.status === AcademicPlanStatus.Draft"
              type="button"
              class="btn-link btn-link-danger"
              @click="$emit('delete', item)"
            >
              حذف
            </button>
          </td>
        </tr>
        <tr v-if="items.length === 0">
          <td colspan="9" class="empty-cell">لا توجد خطط مطابقة</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-responsive {
  width: 100%;
  overflow-x: auto;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 6px;
}
.data-table {
  width: 100%;
  border-collapse: collapse;
  text-align: start;
}
.data-table th,
.data-table td {
  padding: 12px;
  border-bottom: 1px solid #eee;
  font-size: 14px;
}
.data-table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #333;
}
.actions-cell {
  white-space: nowrap;
}
.btn-link {
  background: none;
  border: none;
  color: var(--primary-color, #1e3a8a);
  cursor: pointer;
  padding: 0 6px;
}
.btn-link-danger {
  color: #dc3545;
}
.empty-cell {
  text-align: center;
  color: #6c757d;
}
</style>
